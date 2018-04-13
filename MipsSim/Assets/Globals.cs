using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    public static class Globals
    {
		// filepath to the .s MIPS assembly file.
		public static string filePath;
		
		// program counter
		public static uint PC;

		// next program counter
		public static uint nPC;

		public static dynamic intToDisplay;

		// text parser
		public static Parser parser;

		// Global accessors to registers. Mostly used by UI
		//public static RegisterManager RM = new RegisterManager();
		public static Register hi = new Register("hi", "$hi", 0);
		public static Register lo = new Register("lo", "$lo", 0);

		// Function for incrementing PC and nPC
		public static void AdvancePC(uint offset)
		{
			PC = nPC;
			nPC += offset;
		}

		// Memory Allocation Dictionaries. Memory address will be used for key, instruction for value
		public static Dictionary<uint, uint> textData;
		public static Dictionary<uint, int> staticData; // key will be base memory address. value will be a byte of the data
														 /*
														  * Byte[] temp = BitConverter.GetBytes(0xFF001010);
															Array.Reverse(temp);
															Array.Reverse(temp);
															int tempVal = BitConverter.ToInt32(temp, 0);
															Debug.Log(String.Format("{0:X}", tempVal));
														  * for(int i = 0; i < 4; i++)
														  * staticData[0xx10010000 + i] = temp[i];
														  */
		public static Dictionary<uint, uint> stackData;
	}
}
