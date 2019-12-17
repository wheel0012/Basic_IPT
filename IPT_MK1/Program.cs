using System;
using Basic_IPT.Core;

namespace IPT_MK1
{
    class Program
    {
        static void Main(string[] args)
        {
            string pascal_1 = @"PROGRAM Part10
BEGIN
                                    BEGIN

                                         number := 2
                                         a := number
                                         b := 10 + (23 / 1.5)
                                   END
                                END.";
            string pascal_2 = @"BEGIN
                                    BEGIN
                                         number := 2
                                         a := number
                                         b := 20 * 10
                                   END
                                END.";
            var lexer = new Lexer(pascal_1);
            var parser = new Parser(lexer);
            var ipt = new Interpreter(parser);
            var result = ipt.Process();
            foreach(var item in ipt.GLOBAL_SCOPE)
                Console.WriteLine(item.Key + ":" + item.Value);
            Console.ReadKey();
        }
    }
}
