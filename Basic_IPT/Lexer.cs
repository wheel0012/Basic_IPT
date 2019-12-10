using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Basic_IPT.Core
{
    public enum TokenStatus {IDENTIFIER, STRING, FUNCTION, COMMON, NUMBER, OPERATOR, EOL, SPACE}
    [Flags]
    public enum TokenComp {EMPTY = 0x00, LETTER = 0x01, OPERATOR = 0x02, STRING = 0x04, NUMBER=0x08, SPACE = 0x10 }
    public enum Initiater {INDEX }
    public class Lexer
    {
        private string source_code;
        private Queue<Token> tokens;
        private int tokenStart;
        private int tokenCurrent;
        private int line;
        public Lexer(string code)
        {
            this.source_code = code;
            tokens = new Queue<Token>();
            GenerateTokens();
            
        }
        public Queue<Token> GetTokens()
        {
            return this.tokens;
        }

        public class Token
        {
            public readonly TokenComp status;
            public readonly StringBuilder value;
            internal Token(TokenComp status, StringBuilder value)
            {
                this.status = status;
                this.value = value;
            }
        }
        private void GenerateTokens()
        {
            int index = 0;
            Stack<string> instanceTokenStack = new Stack<string>();
            TokenStatus last = TokenStatus.COMMON;
            while(source_code.Length > index)
            {
                tokens.Enqueue(SeperateToken(ref index));
            }
            tokens.Enqueue(new Token(TokenStatus.EOL, string.Empty));
        }

        private Token SeperateToken(ref int index)
        {
            TokenComp current_status = TokenComp.EMPTY;
            StringBuilder token_value = new StringBuilder();
            for(; )
                token_value.Append(source_code[index]);
                current_status |= JudgeFirstStatus(source_code[index++]);
            return new Token(current_status, token_value);
        }

        private TokenComp JudgeFirstStatus(char ch)
        {
            if (char.IsDigit(ch))
            {
                return TokenComp.NUMBER;
            }
            else if (char.IsLetter(ch))
            {
                return TokenComp.LETTER;
            }
            else if (ch == '\"') return TokenComp.STRING;
            else if (ch == ' ') return TokenComp.SPACE;
            else if (char.IsSymbol(ch)) return TokenComp.OPERATOR;
            return TokenComp.EMPTY;
        }
    }
}