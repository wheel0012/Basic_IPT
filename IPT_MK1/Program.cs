using System;
using Basic_IPT.Core;

namespace IPT_MK1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("IPT_MK1>>");
                Lexer lexer = new Lexer(Console.ReadLine());
                Interpreter ipt = new Interpreter(lexer);
                ipt.Express();
            }
        }
    }
}
