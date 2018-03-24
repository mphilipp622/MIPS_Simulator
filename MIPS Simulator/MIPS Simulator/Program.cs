using System;

namespace MIPS_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
			Parser newParser = new Parser("E:\\College\\MIPS Project\\MIPSSimulator\\MIPS Simulator\\MIPS Simulator\\Test.txt");
			for (int i = 0; i < newParser.GetLines().Count; i++)
				Console.WriteLine(newParser.GetLine());
			//Parser.PrintHelloWorld();
			//Console.WriteLine("Hello World!");
		}
    }
}
