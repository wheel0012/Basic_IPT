using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Basic_IPT
{
    enum Status {IDENTIFIER, STRING, FUNCTION, COMMON, NUMBER, OPERATOR}
    class Lexer
    {
        private string source_code;
        private string[] tokens;
        private int tokenStart;
        private int tokenCurrent;
        private int line;
        public Lexer(string code)
        {
            this.source_code = code;
        }
        public void GenerateToken()
        {
            int index = 0;
            Stack<string> instanceTokenStack = new Stack<string>();
            while(SeperateToken(ref index, ref instanceTokenStack))
            {
                
            }
        }
        private bool SeperateToken(ref int index, ref Stack<string> _stack)
        {
            Status current_status =  Status.COMMON;

            for(;current_status != Status.STRING && source_code[index] != '\r';index++)
            {

            }
            return true;
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
            else return Status.COMMON;

        }
    }
}