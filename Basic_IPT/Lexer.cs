using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Basic_IPT.Core
{
    
    public enum Initiater {INDEX }
    public class Lexer
    {
        private string source_code;
        private int tokenStart;
        private char code_char;
        private int line;
        private int pos;
        public readonly static Dictionary<string, Token> keywords = TokenTool.RegistToken();
            
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
            string str = result.ToString();
            if (keywords.TryGetValue(str, out resultToken)) return resultToken;
            else return new Token(TokenType.ID, str);
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
        private void SkipComments()
        {
            while (code_char != '\n')
            { MovePos(); }
            MovePos();
        }
        private Token GetNumber()
        {
            StringBuilder value = new StringBuilder();
            int intResult;
            float floResult;
            while (char.IsDigit(code_char) && code_char != Symbol.char_null)
            {
                value.Append(code_char);
                MovePos();
            }
            if (code_char == '.')
            {
                value.Append(code_char);
                MovePos();
                while (char.IsDigit(code_char) && code_char != Symbol.char_null)
                {
                    value.Append(code_char);
                    MovePos();
                }
                if (float.TryParse(value.ToString(), out floResult)) return new Token(TokenType.REAL_CONST, floResult);
                throw new FormatException();
            }

            if (int.TryParse(value.ToString(), out intResult)) return new Token(TokenType.INTEGER_CONST, intResult);
            throw new FormatException();
        }
        public Token GetNextToken()
        {
            while (code_char != Symbol.char_null)
            {
                if (code_char == '#')
                {
                    SkipComments();
                    continue;
                }
                if (code_char == ' ')
                {
                    SkipBlanks();
                    continue;
                }

                if (char.IsDigit(this.code_char))
                {
                    return GetNumber();
                }
                if (char.IsLetter(this.code_char))
                {
                    return GetID();
                }
                Token result;
                switch(code_char)
                {
                    case '+':
                        result = new Token(TokenType.PLUS, "+");      
                        MovePos();
                        return result;
                    case '-':
                        result = new Token(TokenType.MINUS, "-");
                        MovePos();
                        return result;
                    case ':':
                        if (this.Peek() == '=')
                        {
                            this.MovePos();
                            this.MovePos();
                            return new Token(TokenType.ASSIGN, ":=");
                        }
                        else
                        {
                            this.MovePos();
                            return new Token(TokenType.COMMA, ",");
                        }
                    case '=':
                        MovePos();
                        return new Token(TokenType.ISEQUAL, "=");
                    case '<':
                        MovePos();
                        return new Token(TokenType.ISLESS, "<");
                    case '>':
                        MovePos();
                        return new Token(TokenType.ISMORE, ">");
                    case '&':
                        MovePos();
                        return new Token(TokenType.AND, "&");
                    case '|':
                        MovePos();
                        return new Token(TokenType.OR, "|");
                    case ',':
                        this.MovePos();
                        return new Token(TokenType.COMMA, ",");
                    case '\r':
                        if (this.Peek() == '\n')
                        {
                            this.MovePos();
                            this.MovePos();
                            return new Token(TokenType.EOL, "EOL");
                        }
                        else continue;
                    case '\n':
                        this.MovePos();
                        return new Token(TokenType.EOL, "EOL");
                    case '.':
                        this.MovePos();
                        return new Token(TokenType.DOT, ".");
                    case '*':
                        result = new Token(TokenType.MUL, "*");
                        MovePos();
                        return result;
                    case '/':
                        result = new Token(TokenType.DIV, "/");
                        this.MovePos();
                        return result;
                    case '(':
                        result = new Token(TokenType.LPAREN, "(");
                        MovePos();
                        return result;
                    case ')':
                        result = new Token(TokenType.RPAREN, ")");
                        MovePos();
                        return result;
                }
            }
            return new Token(TokenType.EOF, string.Empty);
        }

    }
}