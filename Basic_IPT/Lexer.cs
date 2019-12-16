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
        PLUS,
        MINUS,
        STRING, 
        NUMBER, 
        SPACE , 
        EOF,
        LPAREN,
        RPAREN,
        ID,
        IF,
        ELSEIF,
        ELSE,
        THEN,
        RETURN
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
        public int GetPos()
        {
            return this.pos;
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
        private int GetInteger()
        {
            StringBuilder value = new StringBuilder();
            int result;
            while (char.IsDigit(code_char) && code_char != '\u0000')
            {
                value.Append(code_char);
                MovePos();
            }
            if (int.TryParse(value.ToString(), out result)) return result;
            throw new FormatException();
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
                    return new Token(TokenType.NUMBER, GetInteger());
                }
                Token result;
                switch(code_char)
                {
                    case '+':
                        result = new Token(TokenType.PLUS, code_char.ToString());
                        MovePos();
                        return result;
                    case '-':
                        result = new Token(TokenType.MINUS, code_char.ToString());
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
        public readonly object value;
        internal Token(TokenType status, object value)
        {
            this.status = status;
            this.value = value;
        }
    }
}