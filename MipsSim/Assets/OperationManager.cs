﻿using System;
using System.Collections.Generic;

namespace MIPS_Simulator
{
	class OperationManager
    {
		private RegisterManager registers; // stores registers into hash tables. Called on to access registers

		// Delegate definitions. Necessary for creating a hashtable of functions
		delegate void rFunc(byte rs, byte rt, byte rd, byte shamt);
		delegate void iFunc(byte rs, byte rt, short immediate);
		delegate void iFuncU(byte rs, byte rt, ushort immediate);
		delegate void jFunc(uint address);

		// Hashtable definitions
		private Dictionary<Tuple<byte, byte>, rFunc> rFormatOps;
		private Dictionary<byte, iFunc> iFormatOps;
		private Dictionary<byte, iFuncU> iFormatOpsU;
		private Dictionary<byte, jFunc> jFormatOps;

		// Default Constructor
		public OperationManager()
		{
			registers = new RegisterManager();

			// initialize hash tables for functions
			InitRFormat();
			InitIFormat();
			InitJFormat();
		}

		// Executes an operation based on 6-bit opcode and 5-bit funct code.
		public void ExecuteRFormatOp(byte opCode, byte funct, byte rs, byte rt, byte rd, byte shamt)
		{
			Tuple<byte, byte> newOp = new Tuple<byte, byte>(opCode, funct);

			rFormatOps[newOp](rs, rt, rd, shamt);
		}

		// Executes an operation based on 6-bit opcode
		public void ExecuteIFormatOp(byte opCode, byte rs, byte rt, dynamic immediate)
		{
			// figure out if immediate parameter is signed or unsigned.

			if (immediate <= int.MaxValue)
				iFormatOps[opCode](rs, rt, (short)immediate);
			else if (immediate > int.MaxValue)
				iFormatOpsU[opCode](rs, rt, (ushort)immediate);
		}

		public void ExecuteJFormatOp(byte opCode, uint newAddress)
		{
			jFormatOps[opCode](newAddress);
		}

		// Initializes and populates R-Format Dictionary
		private void InitRFormat()
		{
			rFormatOps = new Dictionary<Tuple<byte, byte>, rFunc>();

			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x20), Add);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x21), AddU);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x24), And);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x1A), Div);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x1B), DivU);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x08), JR);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x10), MfHi);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x12), MfLo);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x18), Mult);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x19), Multu);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x27), Nor);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x26), Xor);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x25), Or);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2A), Slt);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2B), SltU);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x00), SLL);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x02), SRL);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x03), SRA);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x22), Sub);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x23), SubU);
		}

		// Initializes and populates I-Format Dictionary
		private void InitIFormat()
		{
			iFormatOps = new Dictionary<byte, iFunc>();
			iFormatOpsU = new Dictionary<byte, iFuncU>();

			// populate signed function hashtable
			iFormatOps.Add(0x08, AddI);
			iFormatOps.Add(0x0C, AndI);
			iFormatOps.Add(0x04, Beq);
			iFormatOps.Add(0x05, Bne);
			iFormatOps.Add(0x0D, OrI);
			iFormatOps.Add(0x0A, SltI);
			//iFormatOps.Add(0x2B, Sw);
			//iFormatOps.Add(0x28, Sb);
			//iFormatOps.Add(0x29, Sh);
			//iFormatOps.Add(0x0F, LuI);
			//iFormatOps.Add(0x23, Lw);

			iFormatOpsU.Add(0x09, AddIU);
			iFormatOpsU.Add(0x0B, SltIU);
			//iFormatOpsU.Add(0x24, lbu);
			//iFormatOpsU.Add(0x25, Lhu);
		}

		// Initializes and Populates J-Format Dictionary
		private void InitJFormat()
		{
			jFormatOps = new Dictionary<byte, jFunc>();

			jFormatOps.Add(0x02, J);
			jFormatOps.Add(0x03, Jal);
		}

		///////////////////////////////////
		/////// R-Format Operations ///////
		///////////////////////////////////

		// All functions have the same parameter lists. This is required so the delegate can be used to call functions
		// from a hashtable. Can't find out a way to allow a dynamic parameter list. This works though.

		// signed Add
		public void Add(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int) registers.registerTable[rs].value + (int) registers.registerTable[rt].value;

			
		}

		// unsigned add
		public void AddU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value + (uint) registers.registerTable[rt].value;

			
		}

		// bitwise and
		void And(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value & registers.registerTable[rt].value;

			
		}

		// signed division
		void Div(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (int)registers.registerTable[rs].value % (int) registers.registerTable[rt].value;
			Globals.lo.value = (int)registers.registerTable[rs].value / (int) registers.registerTable[rt].value;

			
		}

		// unsigned division
		void DivU(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (uint)registers.registerTable[rs].value % (uint) registers.registerTable[rt].value;
			Globals.lo.value = (uint)registers.registerTable[rs].value / (uint) registers.registerTable[rt].value;

			
		}

		// Jump register. Stores rs value into program counter
		void JR(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.PC = Globals.nPC;
			Globals.nPC = registers.registerTable[rs].value;
		}

		// Move From Hi register into rd
		void MfHi(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.hi.value;

			
		}

		// Move from Lo Register into rd
		void MfLo(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.lo.value;

			
		}

		// signed multiplication. Stores significant 32-bits into Hi and lower 32-bits into Lo
		void Mult(byte rs, byte rt, byte rd, byte shamt)
		{
			long product = (int)registers.registerTable[rs].value * (int) registers.registerTable[rt].value;
			var newHi = (int) (product >> 32); // shift top 32 to lowest 32.
			var newLo = (int) (product << 32); // kill leading 32 bits
			newLo = (int)(product >> 32); // shift back to lower 32-bits.

			Globals.hi.value = newHi;
			Globals.lo.value = newLo;

			
		}

		// unsigned multiplication. Stores sig 32-bits into Hi and low 32-bits into Lo
		void Multu(byte rs, byte rt, byte rd, byte shamt)
		{
			ulong product = (uint)registers.registerTable[rs].value * (uint)registers.registerTable[rt].value;
			var newHi = (uint) (product >> 32);
			var newLo = (uint)(product << 32); // kill leading 0's
			newLo = (uint)(product >> 32);

			Globals.hi.value = newHi;
			Globals.lo.value = newLo;

			
		}

		// bitwise nor. rd = not(rs | rt)
		void Nor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ~(registers.registerTable[rs].value | registers.registerTable[rt].value);

			
		}

		// bitwise xor. rd = rs ^ rt
		void Xor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value ^ registers.registerTable[rt].value;

			
		}

		// bitwise or. rd = rs | rt
		void Or(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value | registers.registerTable[rt].value;

			
		}

		// Set register if less than (signed)
		void Slt(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value < (int) registers.registerTable[rt].value;

			
		}

		// Set register if less than (unsigned)
		void SltU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value < (uint)registers.registerTable[rt].value;

			
		}

		// Shift Left Logical
		void SLL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value =  ((uint) registers.registerTable[rt].value) << shamt;

			
		}

		// Shift Right Logical
		void SRL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value = ((uint) registers.registerTable[rt].value) >> shamt;

			
		}

		// Arithmetic Shift Right. Sign Extended.
		void SRA(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses signed numbers for arithmetic shifting.
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> shamt;

			
		}

		// Signed Subtraction. rd = rs - rt
		void Sub(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value - (int) registers.registerTable[rt].value;

			
		}

		// Unsigned Subtraction. rd = rs - rt
		void SubU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value - (uint)registers.registerTable[rt].value;

			
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// signed immediate addition
		void AddI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = (int) (registers.registerTable[rs].value + immediate);

			
		}

		// unsigned immediate addition
		void AddIU(byte rs, byte rt, ushort immediate)
		{
			registers.registerTable[rt].value = (uint)(registers.registerTable[rs].value + immediate);

			
		}

		// Bitwise and immediate. 
		void AndI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value & immediate;

			
		}

		// Branch if Equal. 
		void Beq(byte rs, byte rt, short immediate)
		{
			if (registers.registerTable[rs].value == registers.registerTable[rt].value)
				Globals.AdvancePC((uint) (immediate << 2));
		}

		// Branch if Not Equal.
		void Bne(byte rs, byte rt, short immediate)
		{
			if (registers.registerTable[rs].value != registers.registerTable[rt].value)
				Globals.AdvancePC((uint)(immediate << 2));
		}

		// load byte immediate
		void Lbu(byte rs, byte rt, short immediate)
		{
			
		}

		// Load Halfword Unsigned
		void Lhu()
		{
			
		}

		// Load Upper Immediate
		void LuI()
		{
			
		}

		// Load Word
		void Lw()
		{
			
		}

		// Bitwise Or Immediate
		void OrI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value | immediate;

			
		}

		void Sb()
		{
			
		}

		void Sh()
		{
			
		}

		// Set on less than immediate (signed)
		void SltI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = ((int)registers.registerTable[rs].value) < immediate;

			
		}

		// Set on less than immediate (unsigned)
		void SltIU(byte rs, byte rt, ushort immediate)
		{
			registers.registerTable[rt].value = ((uint)registers.registerTable[rs].value) < immediate;

			
		}

		void Sw()
		{
			
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// Jump command.
		void J(uint address)
		{
			Globals.PC = Globals.nPC;
			Globals.nPC = (Globals.PC & 0xF0000000) | (address << 2);
		}

		// Jump and Link
		void Jal(uint address)
		{
			registers.registerTable[31].value = Globals.PC + 8; // r31 is $ra.
			Globals.PC = Globals.nPC;
			Globals.nPC = (Globals.PC & 0xf0000000) | (address << 2);
		}
	}
}