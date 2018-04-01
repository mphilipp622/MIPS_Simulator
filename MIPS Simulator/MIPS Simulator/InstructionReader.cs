using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    public class InstructionReader
    {
		private RegisterManager registers;

		// default constructor. Initializes registers
		public InstructionReader()
		{
			registers = new RegisterManager();
		}

		public void ParseInstruction(uint instruction)
		{
			uint originalInstruction = instruction; // store original instruction for later

			byte opCode = (byte) (instruction >> 26);

			Console.WriteLine(opCode);
		}

		private void RFormat(uint instruction)
		{
		}

		private void IFormat(uint instruction)
		{

		}

		private void JFormat(uint instruction)
		{

		}
    }
}
