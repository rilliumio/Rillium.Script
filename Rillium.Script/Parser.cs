using Rillium.Script.Expressions;
using Rillium.Script.Statements;

namespace Rillium.Script
{
    internal class Parser
    {
        private readonly Lexer lexer;
        private readonly StreamWriter ouput;
        private readonly FunctionTable functions;

        private Token currentToken;

        public Parser(Lexer lexer, StreamWriter ouput, FunctionTable functions)
        {
            this.lexer = lexer;
            this.ouput = ouput;
            this.functions = functions;
            currentToken = lexer.NextToken();
        }

        public object Parse()
        {
            var scope = new Scope(functions);

            while (true)
            {
                EatSemiColons();
                if (currentToken.Id == TokenId.Eof) { break; }

                var statement = ParseStatement(scope);
                if (statement is ReturnStatement returnStatement)
                {
                    return returnStatement.EvaluateReturnExpression(scope);
                }

                statement.Execute(scope);

                if (currentToken.Id == TokenId.Eof || statement == null) { break; }
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
            while (currentToken.Id == TokenId.Semicolon)
            {
                Eat(TokenId.Semicolon);
            }
        }

        private Expression ParseArithmeticExpression()
        {
            var leftExpr = ParseTerm();

            while (currentToken.Id == TokenId.Plus
                || currentToken.Id == TokenId.Minus
                || currentToken.Id == TokenId.EqualEqual
                || currentToken.Id == TokenId.Less
                || currentToken.Id == TokenId.LessEqual
                || currentToken.Id == TokenId.Greater
                || currentToken.Id == TokenId.GreaterEqual)
            {
                var op = currentToken;
                Eat(op.Id);
                var rightExpr = ParseTerm();
                leftExpr = new BinaryExpression(leftExpr, op, rightExpr);
            }
            return leftExpr;
        }


        private Expression ParseBinaryExpression(Expression leftExpression, int precedence)
        {
            while (true)
            {
                var tokenPrecedence = GetPrecedence(currentToken.Id);

                if (tokenPrecedence < precedence)
                {
                    return leftExpression;
                }

                Eat(currentToken.Id);

                var rightExpression = ParsePrimaryExpression();

                var nextToken = currentToken;
                var nextPrecedence = GetPrecedence(nextToken.Id);

                if (nextPrecedence > tokenPrecedence)
                {
                    rightExpression = ParseBinaryExpression(rightExpression, tokenPrecedence + 1);
                }

                leftExpression = new BinaryExpression(leftExpression, currentToken, rightExpression);
            }
        }

        private int GetPrecedence(TokenId tokenType)
        {
            switch (tokenType)
            {
                case TokenId.Plus:
                case TokenId.Minus:
                    return 1;

                case TokenId.Star:
                case TokenId.Slash:
                    return 2;

                default:
                    return 0;
            }
        }


        // Parse a term
        private Expression ParseTerm()
        {
            var leftFactor = ParseFactor();
            while (currentToken.Id == TokenId.Star || currentToken.Id == TokenId.Slash)
            {
                var operatorToken = currentToken;
                Eat(operatorToken.Id);
                var rightFactor = ParseFactor();
                leftFactor = new BinaryExpression(leftFactor, operatorToken, rightFactor);
            }
            return leftFactor;
        }

        // Parse a factor
        private Expression ParseFactor()
        {
            var isNegative = false;
            if (currentToken.Id == TokenId.Minus)
            {
                isNegative = true;
                Eat(TokenId.Minus);
            }

            if (currentToken.Id == TokenId.Number)
            {
                var numberToken = currentToken;
                Eat(TokenId.Number);

                var d = double.Parse(numberToken.Value);
                if (isNegative) { d = -d; }

                return new NumberExpression(numberToken, d);
            }
            if (currentToken.Id == TokenId.Identifier)
            {
                var identifierExpression = ParseIdentifierExpression();

                return (isNegative)
                    ? new BinaryExpression(
                        new NumberExpression(currentToken, -1),
                        new Token(TokenId.Star, null, currentToken.Line),
                        identifierExpression)
                    : identifierExpression;
            }
            else if (currentToken.Id == TokenId.LeftParen)
            {

                Eat(TokenId.LeftParen);
                var expr = ParseArithmeticExpression();
                Eat(TokenId.RightParen);

                if (isNegative)
                {
                    return new BinaryExpression(
                        new NumberExpression(currentToken, -1),
                        new Token(TokenId.Star, null, currentToken.Line),
                        expr);
                }
                return expr;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid token {currentToken.Value} found " +
                    $"when expecting an integer or a left parenthesis.");
            }
        }

        private DeclarationStatement ParseDeclarationStatement(Scope scope)
        {
            Eat(TokenId.Var);
            var indentifier = currentToken;

            scope.Set(indentifier.Value, null);

            Eat(TokenId.Identifier);

            if (currentToken.Id == TokenId.Semicolon)
            {
                Eat(TokenId.Semicolon);
                return new DeclarationStatement(
                    indentifier,
                    new LiteralExpression(
                        indentifier,
                        new LiteralValue()
                        {
                            Value = null,
                            TypeId = LiteralTypeId.Unknown
                        }));
            }

            Eat(TokenId.Equal);

            // Parse expression
            var initializer = ParseExpression();

            return new DeclarationStatement(indentifier, initializer);
        }

        private ReturnStatement ParseReturnStatement()
        {
            Eat(TokenId.Return);
            return new ReturnStatement(ParseExpression());
        }

        // Parse an if statement
        private IfStatement ParseIfStatement(Scope scope)
        {
            Eat(TokenId.If);
            Eat(TokenId.LeftParen);
            var condition = ParseArithmeticExpression();
            Eat(TokenId.RightParen);


            var thenStatement = ParseBlockStatement(scope);
            Statement elseStatement = null;
            if (currentToken.Id == TokenId.Else)
            {
                Eat(TokenId.Else);
                elseStatement = ParseBlockStatement(scope);
            }
            return new IfStatement(condition, thenStatement, elseStatement);
        }

        // Parse a for loop statement
        private ExpressionStatement ParseIdentifierStatement()
        {
            return new ExpressionStatement(ParseIdentifierExpression());
        }

        // Parse a for loop statement
        private ForLoopStatement ParseForLoopStatement(Scope scope)
        {
            Eat(TokenId.For);
            Eat(TokenId.LeftParen);
            var init = ParseStatement(scope);
            Eat(TokenId.Semicolon);
            var condition = ParseExpression();
            Eat(TokenId.Semicolon);
            var increment = ParseStatement(scope);
            Eat(TokenId.RightParen);
            var body = ParseBlockStatement(scope);
            return new ForLoopStatement(init, condition, increment, body);
        }

        private Expression ParseExpression()
        {
            var expr = ParsePrimaryExpression();
            if (currentToken.Id == TokenId.Comma) { return expr; }

            return ParseToRight(expr);
        }

        private Expression ParseToRight(Expression? expr)
        {
            while (currentToken.Id == TokenId.Plus ||
                   currentToken.Id == TokenId.Minus ||
                   currentToken.Id == TokenId.Star ||
                   currentToken.Id == TokenId.Slash ||
                   currentToken.Id == TokenId.EqualEqual ||
                   currentToken.Id == TokenId.Less ||
                   currentToken.Id == TokenId.LessEqual ||
                   currentToken.Id == TokenId.Greater ||
                   currentToken.Id == TokenId.GreaterEqual)
            {
                var op = currentToken;
                Eat(op.Id);

                var right = ParsePrimaryExpression();
                expr = new BinaryExpression(expr, op, right);
            }

            return expr;
        }

        private Expression? ParsePrimaryExpression()
        {
            var token = currentToken;
            switch (token.Id)
            {
                case TokenId.Identifier:
                    return ParseIdentifierExpression();
                case TokenId.Number:
                case TokenId.Minus:
                    return ParseLiteralExpression();
                case TokenId.LeftParen:
                    return ParseGroupingExpression();
                case TokenId.LeftSquareBracket:
                    return ParseArrayExpression();
                case TokenId.Function:
                    return ParseFunctionExpression();
                default:
                    throw new Exception($"Unexpected token type: {token.Id}");
            }
        }

        private Expression ParseLiteralExpression()
        {
            var isNegative = false;
            if (currentToken.Id == TokenId.Minus)
            {
                isNegative = true;
                Eat(TokenId.Minus);
            }

            var token = currentToken;
            switch (token.Id)
            {
                case TokenId.Number:
                    Eat(TokenId.Number);

                    var d = double.Parse(token.Value);
                    if (isNegative) { d = -d; }
                    return new NumberExpression(token, d);

                case TokenId.LeftParen:
                    var groupingExpressing = ParseGroupingExpression();
                    return (isNegative)
                        ? new BinaryExpression(
                            new NumberExpression(token, -1),
                            new Token(TokenId.Star, null, currentToken.Line),
                            groupingExpressing)
                        : groupingExpressing;

                default:
                    throw new Exception($"Invalid literal expression: {currentToken.Id}");
            }
        }

        private Expression ParseLiteralArrayExpression()
        {
            var isNegative = false;
            if (currentToken.Id == TokenId.Minus)
            {
                isNegative = true;
                Eat(TokenId.Minus);
            }

            var token = currentToken;
            switch (token.Id)
            {
                case TokenId.Number:
                    Eat(TokenId.Number);

                    var d = double.Parse(token.Value);
                    if (isNegative) { d = -d; }
                    return new NumberExpression(token, d);

                case TokenId.LeftParen:
                    var groupingExpressing = ParseGroupingExpression();
                    return (isNegative)
                        ? new BinaryExpression(
                            new NumberExpression(token, -1),
                            new Token(TokenId.Star, null, currentToken.Line),
                            groupingExpressing)
                        : groupingExpressing;

                default:
                    throw new Exception($"Invalid literal expression: {currentToken.Id}");
            }
        }

        private Expression ParseGroupingExpression()
        {
            Eat(TokenId.LeftParen);
            var expr = ParseExpression();
            Eat(TokenId.RightParen);
            return expr;
        }

        private Expression ParseArrayExpression()
        {
            Eat(TokenId.LeftSquareBracket);
            var expressionList = new List<Expression>();
            while (true)
            {
                var expr = ParseLiteralArrayExpression();
                expressionList.Add(expr);

                if (currentToken.Id != TokenId.Comma || currentToken.Id == TokenId.RightSquareBracket)
                {
                    break;
                }

                Eat(TokenId.Comma);
            }

            Eat(TokenId.RightSquareBracket);
            return new ArrayExpression(currentToken, expressionList);
        }


        private BlockStatement ParseBlockStatement(Scope scope)
        {
            var statements = new List<Statement>();

            Eat(TokenId.LeftBrace);

            while (
                currentToken.Id != TokenId.RightBrace &&
                currentToken.Id != TokenId.Semicolon &&
                currentToken.Id != TokenId.Eof)
            {
                statements.Add(ParseStatement(scope));

            }

            while (currentToken.Id == TokenId.Semicolon)
            {
                Eat(TokenId.Semicolon);
            }

            Eat(TokenId.RightBrace);

            return new BlockStatement(statements);
        }

        // Parse a statement
        private Statement? ParseStatement(Scope scope)
        {
            switch (currentToken.Id)
            {
                case TokenId.Var:
                    return ParseDeclarationStatement(scope);
                case TokenId.If:
                    return ParseIfStatement(scope);
                case TokenId.LeftBrace:
                    return ParseBlockStatement(scope);
                case TokenId.For:
                    return ParseForLoopStatement(scope);
                case TokenId.Identifier:
                    return ParseIdentifierStatement();
                case TokenId.Function:
                    return new ExpressionStatement(ParseFunctionExpression());
                case TokenId.Semicolon:
                    return new ExpressionStatement(new LiteralExpression(currentToken, null));
                case TokenId.LeftParen:
                case TokenId.Minus:
                case TokenId.Number:
                    return new ExpressionStatement(ParseArithmeticExpression());
                case TokenId.Return:
                    return ParseReturnStatement();
                case TokenId.Eof:
                    return null;
                default:
                    throw new InvalidOperationException($"Invalid token '{currentToken.Value}' found when expecting a statement");
            }
        }

        private Expression ParseFunctionExpression()
        {
            var functionToken = currentToken;
            Eat(TokenId.Function);
            Eat(TokenId.LeftParen);

            var arguments = new List<Expression>();
            while (currentToken.Id != TokenId.RightParen)
            {
                if (currentToken.Id == TokenId.Comma) { Eat(TokenId.Comma); }

                arguments.Add(ParseExpression());
            }

            Eat(TokenId.RightParen);

            return new FunctionExpression(
                functionToken,
                functionToken.Value, arguments);
        }

        private Expression ParseIdentifierExpression()
        {
            var token = currentToken;
            Eat(TokenId.Identifier);


            if (currentToken.Id == TokenId.Semicolon)
            {
                Eat(TokenId.Semicolon);
                return new VariableExpression(token);
            }


            if (currentToken.Id == TokenId.LeftSquareBracket)
            {
                Eat(TokenId.LeftSquareBracket);
                var indexValueExpression = ParseExpression();
                Eat(TokenId.RightSquareBracket);

                var idxExpr = new IndexExpression(
                            currentToken,
                            new VariableExpression(token),
                            indexValueExpression);

                if (currentToken.Id == TokenId.Semicolon)
                {
                    Eat(TokenId.Semicolon);
                    return idxExpr;
                }

                return ParseToRight(idxExpr);
            }

            if (currentToken.Id == TokenId.Dot)
            {
                Eat(TokenId.Dot);
                var dotName = currentToken;

                EatOne(TokenId.Identifier, TokenId.Function);

                var arraySummaryId = dotName.GetArraySummaryId();
                if (arraySummaryId != ArraySummaryId.Length)
                {
                    Eat(TokenId.LeftParen);
                    Eat(TokenId.RightParen);
                }

                var arrayAggragate = new ArraySummaryExpression(
                           token,
                           new VariableExpression(token),
                           arraySummaryId);

                if (currentToken.Id == TokenId.Semicolon)
                {
                    return arrayAggragate;
                }

                var operatorToken = currentToken;
                Eat(operatorToken.Id);
                return new BinaryExpression(arrayAggragate, operatorToken, ParseExpression());
            }

            if (currentToken.Id == TokenId.Equal)
            {

                Eat(TokenId.Equal);
                var expression = ParseExpression();
                return new AssignmentExpression(token, new VariableExpression(token), expression);
            }

            return ParseToRight(new IdentifierExpression(token));
        }

        // Helper method to advance to the next token
        private void Eat(TokenId expectedType)
        {
            if (currentToken.Id == expectedType)
            {
                currentToken = lexer.NextToken();
            }
            else
            {
                throw new InvalidOperationException($"Invalid token '{currentToken.Value}' found when expecting {expectedType}");
            }
        }

        /// <summary>
        ///     Eats one of the expected tokens.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void EatOne(params TokenId[] expectedTypes)
        {
            foreach (var expectedType in expectedTypes)
            {
                if (currentToken.Id == expectedType)
                {
                    currentToken = lexer.NextToken();
                    return;
                }
            }

            throw new InvalidOperationException(
                $"Invalid token '{currentToken.Value}' found when " +
                $"expecting one of the following: {string.Join(',', $"'{expectedTypes}'")}");
        }
    }
}
