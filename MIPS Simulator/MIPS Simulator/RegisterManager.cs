using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
	// Keeps hash tables for different registers and defines getters for those hash tables.
    class RegisterManager
	{
		// Hash table for all the registers. Key takes a byte value because rs, rt, and rd will only ever take 5 bits
		private Dictionary<byte, Register> _registers;

		// public accessor for the hashtable.
		public Dictionary<byte, Register> registerTable
		{
			get
			{
				return _registers;
			}
		}

		// default constructor will initialize a hash table of all 32 registers with appropriate names and aliases.
		public RegisterManager()
		{
			_registers = new Dictionary<byte, Register>();

			for (byte i = 0; i < 32; i++)
			{
				Register newRegister = null;

				// initialize all registers with names, aliases, and default values of 0.
				if (i == 0)
					newRegister = new Register("R0", "$zero", 0);
				else if (i == 1)
					newRegister = new Register("R1", "$at", 0);
				else if (i == 2 || i == 3)
					newRegister = new Register("R" + i.ToString(), "$v" + (i - 2).ToString(), 0);
				else if (i >= 4 && i <= 7)
					newRegister = new Register("R" + i.ToString(), "$a" + (i - 4).ToString(), 0);
				else if (i >= 8 && i <= 15)
					newRegister = new Register("R" + i.ToString(), "$t" + (i - 8).ToString(), 0);
				else if (i >= 16 && i <= 23)
					newRegister = new Register("R" + i.ToString(), "$s" + (i - 16).ToString(), 0);
				else if (i == 24 || i == 25)
					newRegister = new Register("R" + i.ToString(), "$t" + (i - 16).ToString(), 0);
				else if (i == 26 || i == 27)
					newRegister = new Register("R" + i.ToString(), "$k" + (i - 26).ToString(), 0);
				else if (i == 28)
					newRegister = new Register("R28", "$gp", 0);
				else if (i == 29)
					newRegister = new Register("R29", "$sp", 0);
				else if (i == 30)
					newRegister = new Register("R30", "$fp", 0);
				else if (i == 31)
					newRegister = new Register("R31", "$ra", 0);

				_registers.Add(i, newRegister); 
			}
		}
	}
}
