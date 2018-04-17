using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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

			uint newInst = Convert.ToUInt32(instruction, 16);
			//string hexValue = newInst.ToString("X");
			//newInst = uint.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

			if (GetOpCode(newInst) == 0)
			{
				UIManager.instance.WriteDecodedRFormat(GetOpCode(newInst), GetRS(newInst), GetRT(newInst), GetRD(newInst), GetShamt(newInst), GetFunct(newInst));
			}
			else if (GetOpCode(newInst) == 2 || GetOpCode(newInst) == 3)
				UIManager.instance.WriteDecodedJFormat(GetOpCode(newInst), GetAddress(newInst));
			else if (GetOpCode(newInst) > 3)
				UIManager.instance.WriteDecodedIFormat(GetOpCode(newInst), GetRS(newInst), GetRT(newInst), GetImmediate(newInst));
		}

		public void ParseInstruction()
		{
			//uint newInst = Convert.ToUInt32(instruction, 16);

			UIManager.instance.CloseInputPanel(); // close input panel whenever new instruction is fetched

			//Debug.Log(Globals.PC.ToString("X") + "    " + Globals.textData[(uint)Globals.PC].ToString("X"));
			int newInst = Convert.ToInt32(Globals.textData[(uint) Globals.PC]);

			Globals.PC += 4; // advance program counter as soon as we fetch an instruction

			if (GetOpCode((uint)newInst) == 0)
				RFormat((uint)newInst); // r format op codes always start with 0
			else if (GetOpCode((uint)newInst) == 2 || GetOpCode((uint)newInst) == 3)
				JFormat((uint)newInst);
			else if (GetOpCode((uint)newInst) > 3)
				IFormat((uint)newInst);

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
			instruction = (instruction & 0x03E00000) >> 21;
			//instruction = instruction << 6; // Remove opCode
			//instruction = instruction >> 27; // shift to the right to get 5 bits.

			//return (byte)instruction;
			return (byte)(instruction);
		}

		// parses the instruction and returns register rt byte value
		private byte GetRT(uint instruction)
		{
			instruction = (instruction & 0x001F0000) >> 16;
			//	instruction = instruction << 11; // Remove opCode and rs
			//	instruction = instruction >> 27; // shift to the right to get 5 bits.

			return (byte)(instruction);
		}

		// parses the instruction and returns register rd byte value
		private byte GetRD(uint instruction)
		{
			instruction = (instruction & 0x0000F800) >> 11;
			
			//instruction = instruction << 16; // Remove opCode, rs, and rt
			//instruction = instruction >> 27; // shift to the right to get 5 bits.

			return (byte) instruction;
		}

		// parses the instruction and returns Shamt
		private byte GetShamt(uint instruction)
		{
			instruction = (instruction & 0x000007C0) >> 6;
			//instruction = instruction << 21; // Remove opCode, rs, rt, and rd
			//instruction = instruction >> 27; // shift to the right to get 5 bits.

			return (byte) instruction;
		}

		// parses the instruction and returns funct
		private byte GetFunct(uint instruction)
		{
			//instruction = instruction << 26; // Remove opCode, rs, rt, and rd
			//instruction = instruction >> 26; // shift to the right to get 5 bits.

			return (byte)( instruction & 0x0000003F);
		}

		// parses the instruction and returns IMM
		private dynamic GetImmediate(uint instruction)
		{
			//instruction = instruction << 16; // Remove opCode, rs, and rt
			//instruction = instruction >> 16; // shift to the right to get 5 bits.

			//if (instruction > short.MaxValue)
			//	return (ushort)instruction;

			return (short)(instruction & 0x0000FFFF);
		}

		private uint GetAddress(uint instruction)
		{
			//instruction = instruction << 6; // remove opCode
			//instruction = instruction >> 6;  // shift to right to get 26 bits

			return instruction & 0x03FFFFFF;
		}
	}
}
