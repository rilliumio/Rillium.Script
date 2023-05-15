namespace Rillium.Script
{
    public class SyntaxParser
    {
        private readonly Lexer lexer;
        private Token currentToken;
        private readonly StreamWriter ouput;

        public SyntaxParser(Lexer lexer, StreamWriter ouput)
        {
            this.lexer = lexer;
            this.ouput = ouput;
            currentToken = lexer.NextToken();
        }

        public delegate void StatementHandler(Statement statement);

        public object Parse()
        {
            var scope = new Scope();

            while (true)
            {
                EatSemiColons();
                if (currentToken.Type == TokenType.Eof) { break; }

                var statement = ParseStatement(scope);
                if (statement is ReturnStatement returnStatement)
                {
                    return returnStatement.EvaluateReturnExpression(scope);
                }

                statement.Execute(scope);

                if (currentToken.Type == TokenType.Eof || statement == null) { break; }
                EatSemiColons();

            }

            ouput.Flush();

            if (scope.TryGet(Constants.OutputValueKey, out var outputValue))
            {
                if (outputValue is Expression ex) { return ex.Evaluate(scope); }
                return outputValue;
            }

            return null;
        }

        private void EatSemiColons()
        {
            while (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
            }
        }

        private Expression ParseArithmeticExpression()
        {
            var leftExpr = ParseTerm();

            while (currentToken.Type == TokenType.Plus
                || currentToken.Type == TokenType.Minus
                || currentToken.Type == TokenType.EqualEqual
                || currentToken.Type == TokenType.Less
                || currentToken.Type == TokenType.LessEqual
                || currentToken.Type == TokenType.Greater
                || currentToken.Type == TokenType.GreaterEqual)
            {
                var op = currentToken;
                Eat(op.Type);
                var rightExpr = ParseTerm();
                leftExpr = new BinaryExpression(leftExpr, op.Type, rightExpr);
            }
            return leftExpr;
        }


        private Expression ParseBinaryExpression(Expression leftExpression, int precedence)
        {
            while (true)
            {
                var tokenPrecedence = GetPrecedence(currentToken.Type);

                if (tokenPrecedence < precedence)
                {
                    return leftExpression;
                }

                Eat(currentToken.Type);

                var rightExpression = ParsePrimaryExpression();

                var nextToken = currentToken;
                var nextPrecedence = GetPrecedence(nextToken.Type);

                if (nextPrecedence > tokenPrecedence)
                {
                    rightExpression = ParseBinaryExpression(rightExpression, tokenPrecedence + 1);
                }

                leftExpression = new BinaryExpression(leftExpression, currentToken.Type, rightExpression);
            }
        }

        private int GetPrecedence(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return 1;

                case TokenType.Star:
                case TokenType.Slash:
                    return 2;

                default:
                    return 0;
            }
        }


        // Parse a term
        private Expression ParseTerm()
        {
            var leftFactor = ParseFactor();
            while (currentToken.Type == TokenType.Star || currentToken.Type == TokenType.Slash)
            {
                var op = currentToken;
                Eat(op.Type);
                var rightFactor = ParseFactor();
                leftFactor = new BinaryExpression(leftFactor, op.Type, rightFactor);
            }
            return leftFactor;
        }

        // Parse a factor
        private Expression ParseFactor()
        {
            var isNegative = false;
            if (currentToken.Type == TokenType.Minus)
            {
                isNegative = true;
                Eat(TokenType.Minus);
            }

            if (currentToken.Type == TokenType.Number)
            {
                var numberToken = currentToken;
                Eat(TokenType.Number);

                var d = double.Parse(numberToken.Value);
                if (isNegative) { d = -d; }

                return new NumberExpression(d);
            }
            else if (currentToken.Type == TokenType.LeftParen)
            {

                Eat(TokenType.LeftParen);
                var expr = ParseArithmeticExpression();
                Eat(TokenType.RightParen);

                if (isNegative)
                {
                    return new BinaryExpression(new NumberExpression(-1), TokenType.Star, expr);
                }
                return expr;
            }
            else
            {
                throw new InvalidOperationException($"Invalid token {currentToken.Value} found when expecting an integer or a left parenthesis");
            }
        }

        private DeclarationStatement ParseDeclarationStatement(Scope scope)
        {
            Eat(TokenType.Var);
            var indentifier = currentToken;
            Eat(TokenType.Identifier);

            if (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
                return new DeclarationStatement(
                    indentifier,
                    new LiteralExpression(
                        new LiteralValue()
                        { Value = null, TypeId = LiteralTypeId.Unknown }));
            }

            Eat(TokenType.Equal);

            // Parse expression
            var initializer = ParseExpression();

            return new DeclarationStatement(indentifier, initializer);
        }

        private ReturnStatement ParseReturnStatement()
        {
            Eat(TokenType.Return);
            return new ReturnStatement(ParseExpression());
        }

        // Parse an if statement
        private IfStatement ParseIfStatement(Scope scope)
        {
            Eat(TokenType.If);
            Eat(TokenType.LeftParen);
            var condition = ParseArithmeticExpression();
            Eat(TokenType.RightParen);


            var thenStatement = ParseBlockStatement(scope);
            Statement elseStatement = null;
            if (currentToken.Type == TokenType.Else)
            {
                Eat(TokenType.Else);
                elseStatement = ParseBlockStatement(scope);
            }
            return new IfStatement(condition, thenStatement, elseStatement);
        }

        // Parse a for loop statement
        private ExpressionStatement ParseIdentifierStatement()
        {
            var identifier = currentToken;
            Eat(TokenType.Identifier);

            if (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
                return new ExpressionStatement(new VariableExpression(identifier));
            }

            Eat(TokenType.Equal);
            var expression = ParseExpression();
            var assignmentExpression = new AssignmentExpression(new VariableExpression(identifier), expression);
            return new ExpressionStatement(assignmentExpression);
        }

        // Parse a for loop statement
        private ForLoopStatement ParseForLoopStatement(Scope scope)
        {
            Eat(TokenType.For);
            Eat(TokenType.LeftParen);
            var init = ParseStatement(scope);
            Eat(TokenType.Semicolon);
            var condition = ParseExpression();
            Eat(TokenType.Semicolon);
            var increment = ParseStatement(scope);
            Eat(TokenType.RightParen);
            var body = ParseBlockStatement(scope);
            return new ForLoopStatement(init, condition, increment, body);
        }

        private Statement ParseExpressionStatement()
        {
            var expression = ParseExpression();
            Eat(TokenType.Semicolon);
            return new ExpressionStatement(expression);
        }

        private Expression ParseExpression()
        {
            var expr = ParsePrimaryExpression();

            while (currentToken.Type == TokenType.Plus ||
                   currentToken.Type == TokenType.Minus ||
                   currentToken.Type == TokenType.Star ||
                   currentToken.Type == TokenType.Slash ||
                   currentToken.Type == TokenType.EqualEqual ||
                   currentToken.Type == TokenType.Less ||
                   currentToken.Type == TokenType.LessEqual ||
                   currentToken.Type == TokenType.Greater ||
                   currentToken.Type == TokenType.GreaterEqual)
            {
                var op = currentToken;
                Eat(op.Type);

                var right = ParsePrimaryExpression();
                expr = new BinaryExpression(expr, op.Type, right);
            }

            return expr;
        }

        private Expression? ParsePrimaryExpression()
        {
            var token = currentToken;
            switch (token.Type)
            {
                case TokenType.Identifier:
                    return ParseIdentifierExpression();
                case TokenType.Number:
                case TokenType.Minus:
                    return ParseLiteralExpression();
                case TokenType.LeftParen:
                    return ParseGroupingExpression();
                case TokenType.LeftSquareBracket:
                    return ParseArrayExpression();
                default:
                    throw new Exception($"Unexpected token type: {token.Type}");
            }
        }

        private Expression ParseLiteralExpression()
        {
            var isNegative = false;
            if (currentToken.Type == TokenType.Minus)
            {
                isNegative = true;
                Eat(TokenType.Minus);
            }

            var token = currentToken;
            switch (token.Type)
            {
                case TokenType.Number:
                    Eat(TokenType.Number);

                    var d = double.Parse(token.Value);
                    if (isNegative) { d = -d; }
                    return new NumberExpression(d);

                case TokenType.LeftParen:
                    var groupingExpressing = ParseGroupingExpression();
                    return (isNegative)
                        ? new BinaryExpression(new NumberExpression(-1), TokenType.Star, groupingExpressing)
                        : groupingExpressing;

                default:
                    throw new Exception($"Invalid literal expression: {currentToken.Type}");
            }
        }

        private Expression ParseLiteralArrayExpression()
        {
            var isNegative = false;
            if (currentToken.Type == TokenType.Minus)
            {
                isNegative = true;
                Eat(TokenType.Minus);
            }

            var token = currentToken;
            switch (token.Type)
            {
                case TokenType.Number:
                    Eat(TokenType.Number);

                    var d = double.Parse(token.Value);
                    if (isNegative) { d = -d; }
                    return new NumberExpression(d);

                case TokenType.LeftParen:
                    var groupingExpressing = ParseGroupingExpression();
                    return (isNegative)
                        ? new BinaryExpression(new NumberExpression(-1), TokenType.Star, groupingExpressing)
                        : groupingExpressing;

                default:
                    throw new Exception($"Invalid literal expression: {currentToken.Type}");
            }
        }

        private Expression ParseGroupingExpression()
        {
            Eat(TokenType.LeftParen);
            var expr = ParseExpression();
            Eat(TokenType.RightParen);
            return expr;
        }

        private Expression ParseArrayExpression()
        {
            Eat(TokenType.LeftSquareBracket);
            var expressionList = new List<Expression>();
            while (true)
            {
                var expr = ParseLiteralArrayExpression();
                expressionList.Add(expr);

                if (currentToken.Type != TokenType.Comma || currentToken.Type == TokenType.RightSquareBracket)
                {
                    break;
                }

                Eat(TokenType.Comma);
            }

            Eat(TokenType.RightSquareBracket);
            return new ArrayExpression(expressionList);
        }


        private BlockStatement ParseBlockStatement(Scope scope)
        {
            var statements = new List<Statement>();

            Eat(TokenType.LeftBrace);

            while (
                currentToken.Type != TokenType.RightBrace &&
                currentToken.Type != TokenType.Semicolon &&
                currentToken.Type != TokenType.Eof)
            {
                statements.Add(ParseStatement(scope));

            }

            while (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
            }

            Eat(TokenType.RightBrace);

            return new BlockStatement(statements);
        }

        // Parse a statement
        private Statement? ParseStatement(Scope scope)
        {
            switch (currentToken.Type)
            {
                case TokenType.Var:
                    return ParseDeclarationStatement(scope);
                case TokenType.If:
                    return ParseIfStatement(scope);
                case TokenType.LeftBrace:
                    return ParseBlockStatement(scope);
                case TokenType.For:
                    return ParseForLoopStatement(scope);
                case TokenType.Identifier:
                    return ParseIdentifierStatement();
                case TokenType.Semicolon:
                    return new ExpressionStatement(new LiteralExpression(null));
                case TokenType.LeftParen:
                case TokenType.Minus:
                case TokenType.Number:
                    return new ExpressionStatement(ParseArithmeticExpression());
                case TokenType.Return:
                    return ParseReturnStatement();
                case TokenType.Eof:
                    return null;
                default:
                    throw new InvalidOperationException($"Invalid token '{currentToken.Value}' found when expecting a statement");
            }
        }

        private IdentifierExpression ParseIdentifierExpression()
        {
            var token = currentToken;
            Eat(TokenType.Identifier);
            return new IdentifierExpression(token.Value);
        }

        // Helper method to advance to the next token
        private void Eat(TokenType expectedType)
        {
            if (currentToken.Type == expectedType)
            {
                currentToken = lexer.NextToken();
            }
            else
            {
                throw new InvalidOperationException($"Invalid token '{currentToken.Value}' found when expecting {expectedType}");
            }
        }
    }

    public interface StatementHandler
    {
        void HandleStatement(Statement statement);
    }

    public class PrintStatementHandler : StatementHandler
    {
        public void HandleStatement(Statement statement)
        {
            Console.WriteLine(statement);
        }
    }
}
