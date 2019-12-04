using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Basic_IPT
{
    enum Status {IDENTIFIER, STRING, FUNCTION, COMMON, NUMBER, OPERATOR, EOL, SPACE}
    class Lexer
    {
        private string source_code;
        private Queue<Token> tokens;
        private int tokenStart;
        private int tokenCurrent;
        private int line;
        public Lexer(string code)
        {
            this.source_code = code;
        }
        class Token
        {
            public Status status;
            public StringBuilder value;
            public Token(Status status, StringBuilder value)
            {
                this.status = status;
                this.value = value;
            }
        }
        public void GenerateTokens()
        {
            int index = 0;
            Stack<string> instanceTokenStack = new Stack<string>();
            Status last = Status.COMMON;
            while(index<source_code.Length)
            {
                
                tokens.Enqueue(SeperateToken(ref index));
            }
        }
        private Token SeperateToken(ref int index)
        {
            Status current_status =  JudgeFirstStatus(source_code[index]);
            StringBuilder token_value = new StringBuilder();
            for(;current_status != Status.STRING && source_code[index] != '\r';index++)
            {

            }
            return new Token(current_status, token_value);
        }
        private Status JudgeFirstStatus(char ch)
        {
            if (char.IsDigit(ch))
            {
                return Status.NUMBER;
            }
            else if (char.IsLetter(ch))
            {
                return Status.IDENTIFIER;
            }
            else if (ch == '\"') return Status.STRING;
            else if (ch == ' ') return Status.SPACE;
            else return Status.COMMON;

        }
    }
}