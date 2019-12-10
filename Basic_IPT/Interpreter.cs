using System;
using System.Collections.Generic;

namespace Basic_IPT.Core
{
    public class Interpreter
    {
        string[] source_lines;
        public Interpreter(string source_code)
        {
            source_lines = source_code.Split("\n");
        }
        public object Process()
        {
            for (int i = 0; i < source_lines.Length; i++)
            {
                object result = Execute(new Lexer(source_lines[i]).GetTokens());
                if (result != null)
                {
                    
                    return result;
                }
            }
            return null;
        }
        private object Execute(Queue<Lexer.Token> code)
        {

            var termStack = new Stack<int>();
            var operatorStack = new Stack<string>();
            foreach (var token in code)
            {
                switch (token.status)
                {
                    case TokenComp.NUMBER:
                        termStack.Push(Convert.ToInt16(token.value.ToString()));
                        break;
                    case TokenComp.OPERATOR:
                        operatorStack.Push(token.value.ToString());
                        break;
                }
                if (termStack.Count > 1 && operatorStack.Count>0)
                {
                    int result;
                    switch (operatorStack.Pop())
                    {
                        case "+":
                            result = termStack.Pop() + termStack.Pop();
                            Console.WriteLine("result : " + result);
                            termStack.Push(result);
                            break;
                        case "-":
                            result = termStack.Pop() - termStack.Pop();
                            Console.WriteLine("result : " + result);
                            termStack.Push(result);
                            break;
                    }
                }
                Console.WriteLine(token.status.ToString() + ":" + token.value);
            }
            return 1;
        }
        private void IF_status()
        {

        }
    }
}