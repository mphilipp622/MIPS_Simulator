using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    public class InstructionReader
    {
		private OperationManager opManager;

		// default constructor. Initializes registers
		public InstructionReader()
		{
			opManager = new OperationManager();
		}

		public void ParseAndPrintInstruction(string instruction)
		{
			uint newInst = Convert.ToUInt32(instruction);

			if (GetOpCode(newInst) == 0)
				UIManager.instance.WriteDecodedRFormat(GetOpCode(newInst), GetRS(newInst), GetRT(newInst), GetRS(newInst), GetShamt(newInst), GetFunct(newInst));
			else if (GetOpCode(newInst) == 2 || GetOpCode(newInst) == 3)
				UIManager.instance.WriteDecodedJFormat(GetOpCode(newInst), GetAddress(newInst));
			else if (GetOpCode(newInst) > 3)
				UIManager.instance.WriteDecodedIFormat(GetOpCode(newInst), GetRS(newInst), GetRT(newInst), GetImmediate(newInst));
		}

		public void ParseInstruction(uint instruction)
		{
			Globals.AdvancePC(4); // advance program counter as soon as we fetch an instruction

			if (GetOpCode(instruction) == 0x00)
				RFormat(instruction); // r format op codes always start with 0
		}

		// Parses Registers rs, rt, and rd. Also gets funct code and shamt. Will then call Operation Manager to execute op
		private void RFormat(uint instruction)
		{
			opManager.ExecuteRFormatOp(GetOpCode(instruction), GetFunct(instruction), GetRS(instruction), GetRT(instruction), GetRD(instruction), GetShamt(instruction));
		}

		// Parses Registers rs, rt, and IMM. Will then call Operation Manager to execute op
		private void IFormat(uint instruction)
		{
			opManager.ExecuteIFormatOp(GetOpCode(instruction), GetRS(instruction), GetRT(instruction), GetImmediate(instruction));
		}

		private void JFormat(uint instruction)
		{
			opManager.ExecuteJFormatOp(GetOpCode(instruction), GetAddress(instruction));
		}

		// parses the instruction and returns the opcode.
		private byte GetOpCode(uint instruction)
		{
			return (byte)(instruction >> 26); // get 6 most significant bits for op code.
		}

		// parses the instruction and returns register rs byte value
		private byte GetRS(uint instruction)
		{
			instruction = instruction << 6; // Remove opCode
			instruction = instruction >> 27; // shift to the right to get 5 bits.
			
			return (byte)instruction;
		}

		// parses the instruction and returns register rt byte value
		private byte GetRT(uint instruction)
		{
			instruction = instruction << 11; // Remove opCode and rs
			instruction = instruction >> 27; // shift to the right to get 5 bits.
			
			return (byte)instruction;
		}

		// parses the instruction and returns register rd byte value
		private byte GetRD(uint instruction)
		{
			instruction = instruction << 16; // Remove opCode, rs, and rt
			instruction = instruction >> 27; // shift to the right to get 5 bits.

			return (byte)instruction;
		}

		// parses the instruction and returns Shamt
		private byte GetShamt(uint instruction)
		{
			instruction = instruction << 21; // Remove opCode, rs, rt, and rd
			instruction = instruction >> 27; // shift to the right to get 5 bits.

			return (byte)instruction;
		}

		// parses the instruction and returns funct
		private byte GetFunct(uint instruction)
		{
			instruction = instruction << 26; // Remove opCode, rs, rt, and rd
			instruction = instruction >> 26; // shift to the right to get 5 bits.

			return (byte) instruction;
		}

		// parses the instruction and returns IMM
		private dynamic GetImmediate(uint instruction)
		{
			instruction = instruction << 16; // Remove opCode, rs, and rt
			instruction = instruction >> 16; // shift to the right to get 5 bits.

			if (instruction > short.MaxValue)
				return (ushort)instruction;

			return (short)instruction;
		}

		private uint GetAddress(uint instruction)
		{
			instruction = instruction << 6; // remove opCode
			instruction = instruction >> 6;  // shift to right to get 26 bits

			return instruction;
		}
	}
}
