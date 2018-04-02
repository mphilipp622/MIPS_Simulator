using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    public static class Globals
    {
		// program counter
		public static uint PC;

		// next program counter
		public static uint nPC;

		public static Register hi = new Register("hi", "$hi", 0);
		public static Register lo = new Register("lo", "$lo", 0);

		// Function for incrementing PC and nPC
		public static void AdvancePC(uint offset)
		{
			PC = nPC;
			nPC += offset;
		}
	}
}
