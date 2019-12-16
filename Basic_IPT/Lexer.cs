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
        RETURN,
        ASSIGN,
        DOT,
        BEGIN,
        END,
        VAR,
        EOL
    }
    public enum Initiater {INDEX }
    public class Lexer
    {
        private string source_code;
        private int tokenStart;
        private char code_char;
        private int line;
        private int pos;
        static Dictionary<string, Token> keywords =
            new Dictionary<string, Token>
            {
                {TokenType.IF.ToString(), new Token(TokenType.IF, TokenType.IF.ToString()) },

                {TokenType.THEN.ToString(), new Token(TokenType.THEN, TokenType.THEN.ToString()) },

                {TokenType.ELSE.ToString(), new Token(TokenType.ELSE, TokenType.ELSE.ToString()) },

                {TokenType.ELSEIF.ToString() , new Token(TokenType.ELSEIF, TokenType.ELSEIF.ToString()) },

                {TokenType.RETURN.ToString(), new Token(TokenType.RETURN, TokenType.RETURN.ToString()) }
            };
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
                code_char = Symbol.char_null;
            }
        }
        private Token GetID()
        {
            Token resultToken;
            var result = new StringBuilder();
            while(code_char != Symbol.char_null && char.IsLetterOrDigit(code_char))
            {
                result.Append(code_char);
                MovePos();
            }
            if (keywords.TryGetValue(result.ToString(), out resultToken)) return resultToken;
            throw new Exception("Undefinded keyword error");
        }
        public char Peek()
        {
            var peek_pos = this.pos+1;
            if (peek_pos > this.source_code.Length) return Symbol.char_null;
            else return source_code[peek_pos];
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
            while (char.IsDigit(code_char) && code_char != Symbol.char_null)
            {
                value.Append(code_char);
                MovePos();
            }
            if (int.TryParse(value.ToString(), out result)) return result;
            throw new FormatException();
        }
        public Token GetNextToken()
        {
            while (code_char != Symbol.char_null)
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
                if (char.IsLetter(this.code_char))
                {
                    return GetID();
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
                    case ':':
                        if (this.Peek() == '=')
                        {
                            this.MovePos();
                            this.MovePos();
                            return new Token(TokenType.ASSIGN, ":=");
                        }
                        else continue;
                    case '\r':
                        if (this.Peek() == '\n')
                        {
                            this.MovePos();
                            this.MovePos();
                            return new Token(TokenType.EOL, "EOL");
                        }
                        else continue;
                    case '.':
                        this.MovePos();
                        return new Token(TokenType.DOT, ".");
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