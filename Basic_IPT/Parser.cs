using System;
using System.Collections.Generic;

namespace Basic_IPT.Core
{
    public class AST
    {
    }
    class NoOP : AST
    {

    }
    public class Var :AST
    {
        public readonly Token token;
        public readonly object value;
        public Var(Token token)
        {
            this.token = token;
            this.value = token.value;
        }
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
    public class Compound : AST
    {
        public Stack<object> children;
        public Compound()
        {
            children = new Stack<object>();
        }
    }
    public class Assign : AST
    {
        public object left;
        public Token token;
        public object right;
        public Assign(object left, Token token, object right)
        {
            this.left = left;
            this.token = token;
            this.right = right;
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
            var node = this.Program();
            if (this.curr_token.status != TokenType.EOF)
                throw new Exception("No delimiter error");
            return node;
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
                default:
                    var node = this.Variable();
                    return node;
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
       
        private object CompoundStatement()
        {
            this.GetToken(TokenType.BEGIN);
            var nodes = StatementList();
            this.GetToken(TokenType.END);
            var root = new Compound();
            foreach(var node in nodes)
            {
                root.children.Push(node);
            }
            return root;
        }
        private object Program()
        {
            var node = CompoundStatement();
            GetToken(TokenType.DOT);
            return node;
        }
        private List<object> StatementList()
        {
            var node = Statement();
            var results = new List<object>() { node };
            while (this.curr_token.status == TokenType.EOL)
            {
                this.GetToken(TokenType.EOL);
                results.Add(this.Statement());
            }
            if (curr_token.status == TokenType.ID)
                throw new Exception("No right-node error");
            return results;
        }
        private object Statement()
        {
            object result = null;
            switch(curr_token.status)
            {
                case TokenType.BEGIN:
                    result = this.CompoundStatement();
                    break;
                case TokenType.ID:
                    result = this.AssignmentStatement();
                    break;
                default:
                    result = this.Empty();
                    break;
            }
            return result;
        }
        private object AssignmentStatement()
        {
            var left = this.Variable();
            var token = this.curr_token;
            GetToken(TokenType.ASSIGN);
            var right = this.Express();
            var node = new Assign(left, token, right);
            return node;
        }
        private object Variable()
        {
            var node = new Var(this.curr_token);
            this.GetToken(TokenType.ID);
            return node;
        }
        private object Empty()
        {
            return new NoOP();
        }

    }
}