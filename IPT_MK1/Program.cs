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
                var lexer = new Lexer(Console.ReadLine());
                var parser = new Parser(lexer);
                var ipt = new Interpreter(parser);
                var result = ipt.Process();
                Console.WriteLine(result.ToString());
            }
        }
    }
}
