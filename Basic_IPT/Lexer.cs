using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Basic_IPT.Core
{
    public enum TokenStatus {IDENTIFIER, STRING, FUNCTION, COMMON, NUMBER, OPERATOR, EOL, SPACE}
    public enum TokenType 
    {
        EMPTY ,
        LETTER,
        TERM_OPERATOR,
        EXPR_OPERATOR,
        STRING, 
        NUMBER, 
        SPACE , 
        EOF,
        LPAREN,
        RPAREN
    }
    public enum Initiater {INDEX }
    public class Lexer
    {
        private string source_code;
        private int tokenStart;
        private char code_char;
        private int line;
        private int pos;
        public Lexer(string code)
        {
            this.source_code = code;
            pos = 0;
            code_char = source_code[pos];
        }
        public void MovePos()
        {
            ++pos;
            if (pos <= source_code.Length-1)
            {
                code_char = source_code[(this.pos)];
            }
            else
            {
                code_char = '\u0000';
            }
        }
        private void SkipBlanks()
        {
            while (code_char == ' ')
            { MovePos(); }
        }
        public Token GetNextToken()
        {
            while (code_char != '\u0000')
            {
                if (code_char == ' ')
                {
                    SkipBlanks();
                    continue;
                }

                if (char.IsDigit(this.code_char))
                {
                    StringBuilder numericTerm = new StringBuilder();
                    while (char.IsDigit(source_code[pos]))
                    {
                        numericTerm.Append(source_code[pos]);
                        MovePos();
                        if (pos >= source_code.Length) break;
                    }
                    return new Token(TokenType.NUMBER, numericTerm.ToString());
                }
                Token result;
                switch(code_char)
                {
                    case '+':
                    case '-':
                        result = new Token(TokenType.EXPR_OPERATOR, code_char.ToString());
                        MovePos();
                        return result;
                    case '*':
                    case '/':
                        result = new Token(TokenType.TERM_OPERATOR, code_char.ToString());
                        MovePos();
                        return result;
                    case '(':
                        result = new Token(TokenType.LPAREN, code_char.ToString());
                        MovePos();
                        return result;
                    case ')':
                        result = new Token(TokenType.RPAREN, code_char.ToString());
                        MovePos();
                        return result;
                }
            }
            return new Token(TokenType.EOF, string.Empty);
        }

    }

    public class Token
    {
        public readonly TokenType status;
        public readonly string value;
        internal Token(TokenType status, string value)
        {
            this.status = status;
            this.value = value;
        }
    }
}