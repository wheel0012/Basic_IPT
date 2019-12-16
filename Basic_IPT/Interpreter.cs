using System;
using System.Collections.Generic;
using System.Reflection;

namespace Basic_IPT.Core
{
    public class NodeVisitor
    {
        public object Visit(object node)
        {
            var methodName = "Visit_" + node.GetType().Name;
            Type type = typeof(Interpreter);
            MethodInfo func = type.GetMethod(methodName);
            var result = func.Invoke(obj: this,parameters: new object[] { node });
            return result;
        }
    }
    public class Interpreter : NodeVisitor
    {
        string[] source_lines;
        string source_code;
        Parser parser;
        public Interpreter(Parser parser)
        {
            this.parser= parser;
        }
        public int Visit_Num(object node)
        {
            var typed_node = (Num)node;
            return typed_node.value;
        }
        public int Visit_UnaryOP(object node)
        {
            var typed_node = (UnaryOP)node;
            switch(typed_node.token.status)
            {
                case TokenType.PLUS:
                    return +(int)this.Visit(typed_node.expr);
                case TokenType.MINUS:
                    return -(int)this.Visit(typed_node.expr);
            }
            throw new Exception("Unary type Error" + this.parser.lexer.GetPos());
        }
        public int Visit_BinOP(object node)
        {
            var typed_node = (BinOP)node;
            switch (typed_node.op.status)
            {
                case TokenType.PLUS:
                    return (int)this.Visit(typed_node.left) + (int)this.Visit(typed_node.right);
                case TokenType.MINUS:
                    return (int)this.Visit(typed_node.left) - (int)this.Visit(typed_node.right);
                case TokenType.TERM_OPERATOR:
                    switch(typed_node.op.value)
                    {
                        case "*":
                            return (int)this.Visit(typed_node.left) * (int)this.Visit(typed_node.right);
                        case "/":
                            return (int)this.Visit(typed_node.left) / (int)this.Visit(typed_node.right);
                    }
                    break;
            }
            throw new Exception("Visit_BinOP method error");
        }
        public object Process()
        {
            var ASTtree = this.parser.Parse();
            return this.Visit(ASTtree);
        }/*
        private void GetToken(TokenType type)
        {
            if (type == curr_token.status)
                curr_token = this.lexer.GetNextToken();
            else throw new Exception("Error");
        }
        private int GetTerm(TokenType tokenComp, Token token)
        {
            if (tokenComp == token.status)
            {
                return Convert.ToInt32(token.value);
            }
            else
                throw new FormatException();
        }
        private int Factor()
        {
            var item = curr_token;
            switch(curr_token.status)
            {
                case TokenType.NUMBER:
                    GetToken(TokenType.NUMBER);
                    return GetTerm(TokenType.NUMBER, item);
                case TokenType.LPAREN:
                    GetToken(TokenType.LPAREN);
                    var result = Express();
                    GetToken(TokenType.RPAREN);
                    return result;
            }
            throw new Exception("Expression error");
        }
        private int Term()
        {
            var result = Factor();
            while (curr_token.status == TokenType.TERM_OPERATOR)
            {
                switch (curr_token.value)
                {
                    case "/":
                        this.GetToken(TokenType.TERM_OPERATOR);
                        result = result / this.Factor();
                        Console.WriteLine("result : " + result);
                        break;
                    case "*":
                        this.GetToken(TokenType.TERM_OPERATOR);
                        result = result * this.Factor();
                        Console.WriteLine("result : " + result);
                        break;
                }
            }
            return result;
        }
        public int Express()
        {
            var termStack = new Stack<int>();
            var operatorStack = new Stack<string>();
            var result = Term();
            while(curr_token.status == TokenType.PLUS || curr_token.status == TokenType.MINUS)
            {
                switch (curr_token.status)
                {
                    case TokenType.PLUS:
                        GetToken(TokenType.PLUS);
                        result = result + Term();
                        Console.WriteLine("result : " + result);
                        break;
                    case TokenType.MINUS:
                        GetToken(TokenType.MINUS);
                        result = result - Term();
                        Console.WriteLine("result : " + result);
                        break;
                }
                //Console.WriteLine(token.status.ToString() + ":" + token.value);
            }
            return result;
        }
        private void IF_status()
        {

        }*/
    }
}