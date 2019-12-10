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
            public readonly string value;
            internal Token(TokenComp status, string value)
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
                SeperateToken(ref index);
            }
        }

        private void SeperateToken(ref int index)
        {
            Token currentToken = null;
            if (char.IsDigit(source_code[index]))
            {
                StringBuilder numericTerm = new StringBuilder();
                while (char.IsDigit(source_code[index]))
                {
                    numericTerm.Append(source_code[index++]);
                    if (index >= source_code.Length) break;
                }
                currentToken = new Token(TokenComp.NUMBER, numericTerm.ToString());
            }
            else if (source_code[index] == ' ')
            {
                while(!(source_code[++index] == ' '))
                { }
            }
            else if (source_code[index] == '+') currentToken = new Token(TokenComp.OPERATOR, source_code[index++].ToString());
            else if (source_code[index] == '-') currentToken = new Token(TokenComp.OPERATOR, source_code[index++].ToString());
            if (currentToken != null) tokens.Enqueue(currentToken);
        }

    }
}