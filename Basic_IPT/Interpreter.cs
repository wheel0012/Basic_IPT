using System;
using System.Collections.Generic;

namespace Basic_IPT.Core
{
    public class Interpreter
    {
        string[] source_lines;
        string source_code;
        Token curr_token;
        Lexer lexer;
        public Interpreter(Lexer lexer)
        {
            this.lexer = lexer;
            curr_token = lexer.GetNextToken();
        }
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
            if (curr_token.status == TokenType.NUMBER)
            {
                GetToken(TokenType.NUMBER);
                return GetTerm(TokenType.NUMBER, item);
            }
            else if (curr_token.status == TokenType.LPAREN)
            {
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
            while(curr_token.status == TokenType.EXPR_OPERATOR)
            {
                
                switch (curr_token.status)
                {
                    case TokenType.EXPR_OPERATOR:

                        switch (curr_token.value)
                        {
                            case "+":
                                GetToken(TokenType.EXPR_OPERATOR);
                                result = result + Term();
                                Console.WriteLine("result : " + result);
                                break;
                            case "-":
                                GetToken(TokenType.EXPR_OPERATOR);
                                result = result - Term();
                                Console.WriteLine("result : " + result);
                                break;
                        }
                        break;
                }
                //Console.WriteLine(token.status.ToString() + ":" + token.value);
            }
            return result;
        }
        private void IF_status()
        {

        }
    }
}