using System;

namespace Basic_IPT.Core
{
    public class AST
    {
    }
    class BinOP : AST
    {
        public readonly object left;
        public readonly object right;
        public readonly Token op;
        public BinOP(object left, Token op, object right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
    class Num : AST
    {
        public readonly Token token;
        public readonly int value;
        public Num(Token token)
        {
            this.token = token;
            this.value = Convert.ToInt32(token.value);
        }
    }
    class UnaryOP : AST
    {
        public readonly Token token;
        public readonly object expr;
        public UnaryOP(Token op, object expr)
        {
            this.token = op;
            this.expr = expr;
        }
    }
    public class Parser
    {
        Token curr_token;
        public readonly Lexer lexer;
        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            this.curr_token = lexer.GetNextToken();
        }
        public object Parse()
        {
            return this.Express();
        }
        private object Factor()
        {
            var item = curr_token;
            switch (curr_token.status)
            {
                case TokenType.PLUS:
                    GetToken(TokenType.PLUS);
                    return new UnaryOP(item, this.Factor());
                case TokenType.MINUS:
                    GetToken(TokenType.MINUS);
                    return new UnaryOP(item, this.Factor());
                case TokenType.NUMBER:
                    GetToken(TokenType.NUMBER);
                    return new Num(item);
                case TokenType.LPAREN:
                    GetToken(TokenType.LPAREN);
                    var result = Express();
                    GetToken(TokenType.RPAREN);
                    return result;
            }
            throw new Exception("Expression error");
        }
        private object Term()
        {
            var node = Factor();
            while (curr_token.status == TokenType.TERM_OPERATOR)
            {
                var token = curr_token;
                switch (curr_token.value)
                {
                    case "/":
                        this.GetToken(TokenType.TERM_OPERATOR);
                        break;
                    case "*":
                        this.GetToken(TokenType.TERM_OPERATOR);
                        break;
                }
                node = new BinOP(node, token, Factor());
            }
            return node;
        }
        public object Express()
        {
            var node = Term();
            while (curr_token.status == TokenType.PLUS || curr_token.status == TokenType.MINUS)
            {
                var token = curr_token;
                switch (curr_token.status)
                {
                    case TokenType.PLUS:
                        GetToken(TokenType.PLUS);
                        break;
                    case TokenType.MINUS:
                        GetToken(TokenType.MINUS);
                        break;
                }
                node = new BinOP(node, token, Term());
            }
            return node;
        }
        private void GetToken(TokenType type)
        {
            if (type == curr_token.status)
                curr_token = this.lexer.GetNextToken();
            else throw new Exception("Error");
        }
        
    }
}