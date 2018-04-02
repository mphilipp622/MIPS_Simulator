using System;

namespace MIPS_Simulator
{
	class Program
    {
        static void Main(string[] args)
        {
			Globals.PC = 0;
			InstructionReader test = new InstructionReader();
			uint val = 0b1011_0111_0110_1001_1010_0101_1010_1000;

			Console.WriteLine(test.GetAddress(val));
			//Register temp = test.GetRS(val);
			//Register temp = new Register("R0", "blah", 0);
			//temp.value = int.MaxValue;
			//OperationManager man = new OperationManager();
			//Register temp2 = new Register("R1", "blah2", 0);
			//temp2.value = 123;

			//man.Add(temp, temp2, ref temp2);
			//Console.WriteLine(int.MinValue);
			//Console.WriteLine(temp2.value);
			//RegisterManager registers = new RegisterManager();

			//Console.WriteLine(registers.GetRegister(5).alias);
			//InstructionReader.ParseInstruction(0xFFFFFFFF);
			//Parser newParser = new Parser("E:\\College\\MIPS Project\\MIPSSimulator\\MIPS Simulator\\MIPS Simulator\\Test.txt");
			//for (int i = 0; i < newParser.GetLines().Count; i++)
			//	Console.WriteLine(newParser.GetLine());
			//Parser.PrintHelloWorld();
			//Console.WriteLine("Hello World!");
		}
    }
}
