using Rillium.Script.Exceptions;
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
            this.currentToken = lexer.NextToken();
        }

        public object? Parse(params object[]? args)
        {
            var scope = new Scope(this.functions);
            scope.InitializeScopeArguments(args);

            while (true)
            {
                this.EatSemiColons();
                if (this.currentToken.Id == TokenId.Eof) { break; }

                var statement = this.ParseStatement(scope);
                if (statement is ReturnStatement returnStatement)
                {
                    return returnStatement.EvaluateReturnExpression(scope);
                }

                try
                {
                    statement.Execute(scope);
                }
                catch (ReturnStatementException returnStatementException)
                {
                    return returnStatementException.returnStatement.EvaluateReturnExpression(scope);
                }

                if (this.currentToken.Id == TokenId.Eof || statement == null) { break; }
                this.EatSemiColons();

            }

            this.ouput.Flush();

            if (scope.TryGet(Constants.OutputValueKey, out var outputValue))
            {
                if (outputValue is Expression ex) { return ex.Evaluate(scope); }
                return outputValue;
            }

            return null;
        }

        private void EatSemiColons()
        {
            while (this.currentToken.Id == TokenId.Semicolon)
            {
                this.Eat(TokenId.Semicolon);
            }
        }

        private Expression ParseArithmeticExpression()
        {
            var leftExpr = this.ParseTerm();
            if (this.currentToken.Id == TokenId.RightParen)
            {
                return leftExpr;
            }

            if (this.currentToken.Id == TokenId.Number)
            {
                throw new SyntaxException($"Line {this.currentToken.Line + 1}. Syntax error. Expected operator.");
            }

            while (this.currentToken.Id == TokenId.Plus
                || this.currentToken.Id == TokenId.Minus
                || this.currentToken.Id == TokenId.EqualEqual
                || this.currentToken.Id == TokenId.BangEqual
                || this.currentToken.Id == TokenId.Less
                || this.currentToken.Id == TokenId.LessEqual
                || this.currentToken.Id == TokenId.Greater
                || this.currentToken.Id == TokenId.GreaterEqual)
            {
                var op = this.currentToken;
                this.Eat(op.Id);
                var rightExpr = this.ParseTerm();
                leftExpr = new BinaryExpression(leftExpr, op, rightExpr);
            }
            return leftExpr;
        }

        // Parse a term
        private Expression ParseTerm()
        {
            var leftFactor = this.ParseFactor();

            if (this.currentToken.Id == TokenId.RightParen)
            {
                return leftFactor;
            }

            while (this.currentToken.Id == TokenId.Star || this.currentToken.Id == TokenId.Slash || this.currentToken.Id == TokenId.Percent)
            {
                var operatorToken = this.currentToken;
                this.Eat(operatorToken.Id);
                var rightFactor = this.ParseFactor();
                leftFactor = new BinaryExpression(leftFactor, operatorToken, rightFactor);
            }
            return leftFactor;
        }

        // Parse a factor
        private Expression ParseFactor()
        {
            var isNegative = false;

            if (this.currentToken.Id == TokenId.Minus)
            {
                isNegative = true;
                this.Eat(TokenId.Minus);
            }

            if (this.currentToken.Id == TokenId.True)
            {
                this.Eat(TokenId.True);
                return this.currentToken.BuildLiteralExpression(LiteralTypeId.Bool, true);
            }

            if (this.currentToken.Id == TokenId.Number)
            {
                var numberToken = this.currentToken;
                this.Eat(TokenId.Number);

                if (!double.TryParse(numberToken.Value, out var d))
                {
                    throw new SyntaxException($"Line {numberToken.Line + 1}. Syntax error. Invalid number.");
                }

                if (isNegative) { d = -d; }

                return new NumberExpression(numberToken, d);
            }

            if (this.currentToken.Id == TokenId.Function)
            {
                return this.ParseFunctionExpression();
            }

            if (this.currentToken.Id == TokenId.Identifier)
            {
                var identifierExpression = this.ParseIdentifierExpression();

                return (isNegative)
                    ? new BinaryExpression(
                        new NumberExpression(this.currentToken, -1),
                        new Token(TokenId.Star, null, this.currentToken.Line),
                        identifierExpression)
                    : identifierExpression;
            }
            else if (this.currentToken.Id == TokenId.LeftParen)
            {

                this.Eat(TokenId.LeftParen);
                var expr = this.ParseExpression();
                this.Eat(TokenId.RightParen);

                if (isNegative)
                {
                    return new BinaryExpression(
                        new NumberExpression(this.currentToken, -1),
                        new Token(TokenId.Star, null, this.currentToken.Line),
                        expr);
                }
                return expr;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid token {this.currentToken.Value} found " +
                    $"when expecting an integer or a left parenthesis.");
            }
        }

        private DeclarationStatement ParseDeclarationStatement(Scope scope)
        {
            this.Eat(TokenId.Var);
            var identifier = this.currentToken;

            scope.Set(identifier.Value, null);

            this.Eat(TokenId.Identifier);

            if (this.currentToken.Id == TokenId.Semicolon)
            {
                this.EatSemiColons();
                return new DeclarationStatement(
                    identifier,
                    new LiteralExpression(
                        identifier,
                        new LiteralValue()
                        {
                            Value = null,
                            TypeId = LiteralTypeId.UnAssigned
                        }));
            }

            this.Eat(TokenId.Equal);

            // Parse expression
            var initializer = this.ParseExpression();

            return new DeclarationStatement(identifier, initializer);
        }

        private ReturnStatement ParseReturnStatement()
        {
            this.Eat(TokenId.Return);
            return new ReturnStatement(this.ParseExpression());
        }

        // Parse an if statement
        private IfStatement ParseIfStatement(Scope scope)
        {
            this.Eat(TokenId.If);
            this.Eat(TokenId.LeftParen);
            var condition = this.ParseArithmeticExpression();
            this.Eat(TokenId.RightParen);


            var thenStatement = this.ParseBlockStatement(scope);
            Statement? elseStatement = null;
            if (this.currentToken.Id == TokenId.Else)
            {
                this.Eat(TokenId.Else);
                elseStatement = this.ParseBlockStatement(scope);
            }
            return new IfStatement(condition, thenStatement, elseStatement);
        }

        // Parse a for loop statement
        private ForLoopStatement ParseForLoopStatement(Scope scope)
        {
            this.Eat(TokenId.For);
            this.Eat(TokenId.LeftParen);
            var init = this.ParseStatement(scope);
            this.EatSemiColons();
            var condition = this.ParseExpression();
            this.EatSemiColons();
            var increment = this.ParseStatement(scope);
            this.Eat(TokenId.RightParen);
            var body = this.ParseBlockStatement(scope);
            return new ForLoopStatement(init, condition, increment, body);
        }

        private Expression ParseExpression()
        {
            if (this.currentToken.Id == TokenId.Number)
            {
                return this.ParseArithmeticExpression();
            }

            var expr = this.ParsePrimaryExpression();
            if (this.currentToken.Id == TokenId.Comma) { return expr; }

            return this.ParseToRight(expr);
        }

        private Expression ParseToRight(Expression? expr)
        {
            if (this.currentToken.Id == TokenId.RightParen)
            {
                return expr;
            }

            while (this.currentToken.Id == TokenId.Plus ||
                   this.currentToken.Id == TokenId.Minus ||
                   this.currentToken.Id == TokenId.Star ||
                   this.currentToken.Id == TokenId.Slash ||
                   this.currentToken.Id == TokenId.EqualEqual ||
                   this.currentToken.Id == TokenId.BangEqual ||
                   this.currentToken.Id == TokenId.Less ||
                   this.currentToken.Id == TokenId.LessEqual ||
                   this.currentToken.Id == TokenId.Greater ||
                   this.currentToken.Id == TokenId.GreaterEqual)
            {
                var op = this.currentToken;
                this.Eat(op.Id);

                var right = this.ParseToRight(this.ParsePrimaryExpression());
                expr = new BinaryExpression(expr, op, right);
            }

            return expr;
        }

        private Expression? ParsePrimaryExpression()
        {
            var token = this.currentToken;
            switch (token.Id)
            {
                case TokenId.PlusPlus:
                case TokenId.MinusMinus:
                case TokenId.Identifier:
                    return this.ParseIdentifierExpression();
                case TokenId.String:
                case TokenId.Number:
                case TokenId.Minus:
                case TokenId.True:
                case TokenId.False:
                    return this.ParseLiteralExpression();
                case TokenId.LeftParen:
                    return this.ParseGroupingExpression();
                case TokenId.LeftSquareBracket:
                    return this.ParseArrayExpression();
                case TokenId.Function:
                    return this.ParseFunctionExpression();
                default:
                    throw new Exception($"Unexpected token type: {token.Id}");
            }
        }

        private Expression ParseLiteralExpression()
        {
            var isNegative = false;
            if (this.currentToken.Id == TokenId.Minus)
            {
                isNegative = true;
                this.Eat(TokenId.Minus);
            }

            var token = this.currentToken;
            switch (token.Id)
            {
                case TokenId.String:
                case TokenId.True:
                case TokenId.False:
                    this.Eat(token.Id);
                    return token.BuildLiteralExpression();

                case TokenId.Number:
                    this.Eat(TokenId.Number);

                    var d = double.Parse(token.Value);
                    if (isNegative) { d = -d; }
                    return new NumberExpression(token, d);

                case TokenId.LeftParen:
                    var groupingExpressing = this.ParseGroupingExpression();
                    return (isNegative)
                        ? new BinaryExpression(
                            new NumberExpression(token, -1),
                            new Token(TokenId.Star, null, this.currentToken.Line),
                            groupingExpressing)
                        : groupingExpressing;

                default:
                    throw new ScriptException($"Invalid literal expression: {this.currentToken.Id}");
            }
        }

        private Expression ParseGroupingExpression()
        {
            this.Eat(TokenId.LeftParen);
            var expr = this.ParseExpression();
            this.Eat(TokenId.RightParen);
            return expr;
        }

        private Expression ParseArrayExpression()
        {
            this.Eat(TokenId.LeftSquareBracket);
            var expressionList = new List<Expression>();
            while (true)
            {
                var expr = this.ParseLiteralExpression();
                expressionList.Add(expr);

                if (this.currentToken.Id != TokenId.Comma || this.currentToken.Id == TokenId.RightSquareBracket)
                {
                    break;
                }

                this.Eat(TokenId.Comma);
            }

            this.Eat(TokenId.RightSquareBracket);
            return new ArrayExpression(this.currentToken, expressionList);
        }

        private BlockStatement ParseBlockStatement(Scope scope)
        {
            var statements = new List<Statement>();

            this.Eat(TokenId.LeftBrace);
            this.EatSemiColons();
            while (
                this.currentToken.Id != TokenId.RightBrace &&
                this.currentToken.Id != TokenId.Semicolon &&
                this.currentToken.Id != TokenId.Eof)
            {
                statements.Add(this.ParseStatement(scope)!);
            }

            this.EatSemiColons();
            this.Eat(TokenId.RightBrace);

            return new BlockStatement(statements);
        }

        // Parse a statement
        private Statement? ParseStatement(Scope scope)
        {
            switch (this.currentToken.Id)
            {
                case TokenId.Var:
                    return this.ParseDeclarationStatement(scope);
                case TokenId.If:
                    return this.ParseIfStatement(scope);
                case TokenId.LeftBrace:
                    return this.ParseBlockStatement(scope);
                case TokenId.For:
                    return this.ParseForLoopStatement(scope);
                case TokenId.Identifier:
                case TokenId.PlusPlus:
                case TokenId.MinusMinus:
                    return new ExpressionStatement(this.ParseIdentifierExpression());
                case TokenId.Function:
                    return new ExpressionStatement(this.ParseFunctionExpression());
                case TokenId.Semicolon:
                    return new ExpressionStatement(new LiteralExpression(this.currentToken, null));
                case TokenId.LeftParen:
                case TokenId.Minus:
                case TokenId.Number:
                    return new ExpressionStatement(this.ParseArithmeticExpression());
                case TokenId.String:
                case TokenId.True:
                case TokenId.False:
                    return new ExpressionStatement(this.ParseExpression());
                case TokenId.Return:
                    return this.ParseReturnStatement();
                case TokenId.Eof:
                    return null;
                default:
                    throw new SyntaxException(
                        $"Line {this.currentToken.Line + 1}. Invalid token '{this.currentToken.Id}' " +
                        $"found when expecting a statement");
            }
        }

        private Expression ParseFunctionExpression()
        {
            var functionToken = this.currentToken;

            this.Eat(TokenId.Function);
            this.Eat(TokenId.LeftParen);

            var arguments = new List<Expression>();
            while (this.currentToken.Id != TokenId.RightParen)
            {
                if (this.currentToken.Id == TokenId.Comma) { this.Eat(TokenId.Comma); }

                arguments.Add(this.ParseExpression());
            }

            this.Eat(TokenId.RightParen);

            return this.ParseToRight(
                new FunctionExpression(
                    functionToken,
                    functionToken.Value!,
                    arguments));
        }

        private Expression ParseIdentifierExpression()
        {
            TokenId? preToken = null;
            if (this.currentToken.Id == TokenId.PlusPlus ||
                this.currentToken.Id == TokenId.MinusMinus)
            {
                preToken = this.currentToken.Id;
                this.Eat(this.currentToken.Id);
            }

            var token = this.currentToken;
            token.PreToken = preToken;

            this.Eat(TokenId.Identifier);

            if (this.currentToken.Id == TokenId.Semicolon)
            {
                this.EatSemiColons();
                return new VariableExpression(token);
            }


            if (this.currentToken.Id == TokenId.LeftSquareBracket)
            {
                this.Eat(TokenId.LeftSquareBracket);
                var indexValueExpression = this.ParseExpression();
                this.Eat(TokenId.RightSquareBracket);

                var idxExpr = new IndexExpression(
                            this.currentToken,
                            new VariableExpression(token),
                            indexValueExpression);

                if (this.currentToken.Id == TokenId.Semicolon)
                {
                    this.EatSemiColons();
                    return idxExpr;
                }

                return this.ParseToRight(idxExpr);
            }

            if (this.currentToken.Id == TokenId.Dot)
            {
                this.Eat(TokenId.Dot);
                var dotName = this.currentToken;

                this.EatOne(TokenId.Identifier, TokenId.Function);

                var arraySummaryId = dotName.GetArraySummaryId();
                if (arraySummaryId != ArraySummaryId.Length)
                {
                    this.Eat(TokenId.LeftParen);
                    this.Eat(TokenId.RightParen);
                }

                var arrayAggregate = new ArraySummaryExpression(
                           token,
                           new VariableExpression(token),
                           arraySummaryId);

                if (this.currentToken.Id == TokenId.Semicolon)
                {
                    return arrayAggregate;
                }

                var operatorToken = this.currentToken;
                this.Eat(operatorToken.Id);
                return new BinaryExpression(arrayAggregate, operatorToken, this.ParseExpression());
            }

            if (this.currentToken.Id == TokenId.Equal)
            {

                this.Eat(TokenId.Equal);
                var expression = this.ParseExpression();
                return new AssignmentExpression(token, new VariableExpression(token), expression);
            }

            var nextToken = this.currentToken;
            if (nextToken.Id == TokenId.PlusPlus || nextToken.Id == TokenId.MinusMinus)
            {
                this.EatOne(TokenId.PlusPlus, TokenId.MinusMinus);
            }

            return this.ParseToRight(new IdentifierExpression(token, nextToken.Id));
        }

        // Helper method to advance to the next token
        private void Eat(TokenId expectedType)
        {
            if (this.currentToken.Id == expectedType)
            {
                this.currentToken = this.lexer.NextToken();
            }
            else
            {
                throw new InvalidOperationException($"Invalid token '{this.currentToken.Value}' found when expecting {expectedType}");
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
                if (this.currentToken.Id == expectedType)
                {
                    this.currentToken = this.lexer.NextToken();
                    return;
                }
            }

            throw new InvalidOperationException(
                $"Invalid token '{this.currentToken.Value}' found when " +
                $"expecting one of the following: {string.Join(',', $"'{expectedTypes}'")}");
        }
    }
}
