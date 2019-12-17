using System;
using System.Collections.Generic;

namespace Basic_IPT.Core
{

    public partial class Parser
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
                case TokenType.INTEGER_CONST:
                    GetToken(TokenType.INTEGER_CONST);
                    return new Num(item);
                case TokenType.REAL_CONST:
                    GetToken(TokenType.REAL_CONST);
                    return new Real(item);
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
            while (curr_token.status == TokenType.MUL || curr_token.status == TokenType.DIV)
            {
                var token = curr_token;
                switch (curr_token.status)
                {
                    case TokenType.MUL:
                        this.GetToken(TokenType.MUL);
                        break;
                    case TokenType.DIV:
                        this.GetToken(TokenType.DIV);
                        break;/*
                    case TokenType.FLOAT_DIV:
                        this.GetToken(TokenType.FLOAT_DIV);
                        break;*/
                }
                node = MakeOperation(node, token, Factor());
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
                node = MakeOperation(node, token, Term());
            }
            return node;
        }
        private object MakeOperation(object node_1, Token token, object node_2)
        {
            object result = null;
            if (node_1.GetType().Equals(typeof(Real)) || node_2.GetType().Equals(typeof(Real)))
            {
                result = new FloatOP(node_1, token, node_2);
            }
            else
            {
                result = new BinOP(node_1, token, node_2);
            }
            return result;
        }
        private void GetToken(TokenType type)
        {
            if (type == curr_token.status)
                curr_token = this.lexer.GetNextToken();
            else throw new Exception("Error");
        }
       
        
        private object Program()
        {
            this.GetToken(TokenType.PROGRAM);
            var var_node = this.Variable();
            var prog_name = var_node.value.ToString();
            this.GetToken(TokenType.EOL);
            var block_node = this.Block();
            var program_node = new Program(prog_name, block_node);
            GetToken(TokenType.DOT);
            return program_node;
        }
        private object Empty()
        {
            return new NoOP();
        }

    }
}