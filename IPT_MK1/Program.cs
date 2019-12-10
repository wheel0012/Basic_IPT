using System;
using Basic_IPT.Core;

namespace IPT_MK1
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter ipt = new Interpreter("1+1-1");
            ipt.Process();
            Console.WriteLine("Hello World!");
        }
    }
}
