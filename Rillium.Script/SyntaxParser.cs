namespace Rillium.Script
{
    public class SyntaxParser
    {
        private readonly Lexer lexer;
        private Token currentToken;
        private IDictionary<string, LiteralValue> _vars;

        public SyntaxParser(Lexer lexer)
        {
            this.lexer = lexer;
            currentToken = lexer.NextToken();
        }

        public List<Statement> ParseStatements()
        {
            var statements = new List<Statement>();

            while (currentToken.Type != TokenType.Eof)
            {
                EatSemiColons();

                statements.Add(ParseStatement());

                EatSemiColons();
            }

            return statements;
        }

        private void EatSemiColons()
        {
            while (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
            }
        }

        // Parse an arithmetic expression
        public Expression ParseArithmeticExpression()
        {
            var leftExpr = ParseTerm();

            while (currentToken.Type == TokenType.Plus
                || currentToken.Type == TokenType.Minus
                || currentToken.Type == TokenType.EqualEqual)
            {
                var op = currentToken;
                Eat(op.Type);
                var rightExpr = ParseTerm();
                leftExpr = new BinaryExpression(leftExpr, currentToken.Type, rightExpr);
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
                leftFactor = new BinaryExpression(leftFactor, currentToken.Type, rightFactor);
            }
            return leftFactor;
        }

        // Parse a factor
        private Expression ParseFactor()
        {
            if (currentToken.Type == TokenType.Number)
            {
                var numberToken = currentToken;
                Eat(TokenType.Number);

                return new NumberExpression(double.Parse(numberToken.Value));
            }
            else if (currentToken.Type == TokenType.LeftParen)
            {
                Eat(TokenType.LeftParen);
                var expr = ParseArithmeticExpression();
                Eat(TokenType.RightParen);
                return expr;
            }
            else
            {
                throw new InvalidOperationException($"Invalid token {currentToken.Value} found when expecting an integer or a left parenthesis");
            }
        }

        public DeclarationStatement ParseDeclarationStatement()
        {
            Eat(TokenType.Var);
            var indentifier = currentToken;
            Eat(TokenType.Identifier);
            Eat(TokenType.Equal);

            // Parse expression
            var initializer = ParseExpression();

            return new DeclarationStatement(indentifier, initializer);
        }

        // Parse an if statement
        public IfStatement ParseIfStatement()
        {
            Eat(TokenType.If);
            Eat(TokenType.LeftParen);
            var condition = ParseArithmeticExpression();
            Eat(TokenType.RightParen);


            var thenStatement = ParseBlockStatement();
            Statement elseStatement = null;
            if (currentToken.Type == TokenType.Else)
            {
                Eat(TokenType.Else);
                elseStatement = ParseBlockStatement();
            }
            return new IfStatement(condition, thenStatement, elseStatement);
        }

        // Parse a for loop statement
        public ExpressionStatement ParseIdentifierStatement()
        {
            var identifier = currentToken;
            Eat(TokenType.Identifier);

            if (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
                var ex = ParseExpression();
                return new ExpressionStatement(ex);
            }

            Eat(TokenType.Equal);
            var expression = ParseExpression();
            var assignmentExpression = new AssignmentExpression(new VariableExpression(identifier), expression);
            return new ExpressionStatement(assignmentExpression);
        }

        // Parse a for loop statement
        public ForLoopStatement ParseForLoopStatement()
        {
            Eat(TokenType.For);
            Eat(TokenType.LeftParen);
            var init = ParseStatement();
            Eat(TokenType.Semicolon);
            var condition = ParseExpression();
            Eat(TokenType.Semicolon);
            var increment = ParseStatement();
            Eat(TokenType.RightParen);
            var body = ParseBlockStatement();
            return new ForLoopStatement(init, condition, increment, body);
        }

        private Statement ParseExpressionStatement()
        {
            var expression = ParseExpression();
            Eat(TokenType.Semicolon);
            return new ExpressionStatement(expression);
        }

        public Expression ParseExpression()
        {
            var expr = ParsePrimaryExpression();

            while (currentToken.Type == TokenType.Plus ||
                   currentToken.Type == TokenType.Minus ||
                   currentToken.Type == TokenType.Star ||
                   currentToken.Type == TokenType.Slash)
            {
                var op = currentToken;
                Eat(op.Type);

                var right = ParsePrimaryExpression();
                var b = new BinaryExpression(expr, op.Type, right);
                var reduced = b.TryReduce();
                expr = (reduced != null) ? reduced : b;
            }

            return expr;
        }

        private Expression ParsePrimaryExpression()
        {
            var token = currentToken;
            switch (token.Type)
            {
                case TokenType.Identifier:
                    return ParseIdentifierExpression();
                case TokenType.Number:
                    return ParseLiteralExpression();
                case TokenType.LeftParen:
                    return ParseGroupingExpression();
                default:
                    throw new Exception($"Unexpected token type: {token.Type}");
            }
        }

        private LiteralExpression ParseLiteralExpression()
        {
            var token = currentToken;
            switch (token.Type)
            {
                case TokenType.Number:
                    Eat(TokenType.Number);
                    return new LiteralExpression(
                        new LiteralValue()
                        {
                            TypeId = LiteralTypeId.Number,
                            Value = token.Value
                        });
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


        private BlockStatement ParseBlockStatement()
        {
            var statements = new List<Statement>();

            Eat(TokenType.LeftBrace);

            while (
                currentToken.Type != TokenType.RightBrace &&
                currentToken.Type != TokenType.Semicolon &&
                currentToken.Type != TokenType.Eof)
            {
                statements.Add(ParseStatement());

            }

            while (currentToken.Type == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
            }

            Eat(TokenType.RightBrace);

            return new BlockStatement(statements);
        }

        // Parse a statement
        public Statement ParseStatement()
        {
            switch (currentToken.Type)
            {
                case TokenType.Var:
                    return ParseDeclarationStatement();
                case TokenType.If:
                    return ParseIfStatement();
                case TokenType.LeftBrace:
                    return ParseBlockStatement();
                case TokenType.For:
                    return ParseForLoopStatement();
                case TokenType.Identifier:
                    return ParseIdentifierStatement();
                case TokenType.Semicolon:
                    return new ExpressionStatement(new LiteralExpression(null));
                case TokenType.Eof:
                    return new ExpressionStatement(new LiteralExpression(null));
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

}
