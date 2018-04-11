using System;
using System.Collections.Generic;

namespace MIPS_Simulator
{
	// Basically decodes all the operations
	class OperationManager
    {
		private RegisterManager registers; // stores registers into hash tables. Called on to access registers
		private RegisterTextManager textMan;

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
			textMan = new RegisterTextManager();

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
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x09), JALR);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x10), MfHi);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x11), MtHi);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x12), MfLo);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x13), MtLo);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x18), Mult);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x19), Multu);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x27), Nor);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x26), Xor);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x25), Or);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2A), Slt);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2B), SltU);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x00), SLL);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x04), SLLV);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x02), SRL);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x06), SRLV);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x03), SRA);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x07), SRAV);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x22), Sub);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x23), SubU);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xA), MovZ);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xB), MovN);
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xC), SysCall);
		}

		// Initializes and populates I-Format Dictionary
		private void InitIFormat()
		{
			iFormatOps = new Dictionary<byte, iFunc>();
			iFormatOpsU = new Dictionary<byte, iFuncU>();

			// populate signed function hashtable
			iFormatOpsU.Add(0x04, Beq);
			iFormatOpsU.Add(0x05, Bne);
			iFormatOpsU.Add(0x06, Blez);
			iFormatOpsU.Add(0x07, Bgtz);
			iFormatOps.Add(0x08, AddI);
			iFormatOps.Add(0x0C, AndI);
			iFormatOps.Add(0x0D, OrI);
			iFormatOps.Add(0x0F, XorI);
			iFormatOps.Add(0x0A, SltI);
			iFormatOps.Add(0x20, Lb);
			iFormatOps.Add(0x21, Lh);
			iFormatOps.Add(0x23, Lw);
			iFormatOps.Add(0x2B, Sw);
			iFormatOps.Add(0x28, Sb);
			iFormatOps.Add(0x29, Sh);
			iFormatOps.Add(0x0F, LuI);

			iFormatOpsU.Add(0x09, AddIU);
			iFormatOpsU.Add(0x0B, SltIU);
			iFormatOps.Add(0x24, Lbu);
			iFormatOps.Add(0x25, Lhu);
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

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// unsigned add
		public void AddU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value + (uint) registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// bitwise and
		void And(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value & registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// signed division
		void Div(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (int)registers.registerTable[rs].value % (int) registers.registerTable[rt].value;
			Globals.lo.value = (int)registers.registerTable[rs].value / (int) registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// unsigned division
		void DivU(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (uint)registers.registerTable[rs].value % (uint) registers.registerTable[rt].value;
			Globals.lo.value = (uint)registers.registerTable[rs].value / (uint) registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// Jump register. Stores rs value into program counter
		void JR(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.PC = registers.registerTable[rs].value;
			//Globals.nPC = registers.registerTable[rs].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// jump and link register
		void JALR(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[31].value = Globals.PC + 4;
			Globals.PC = registers.registerTable[rs].value;
		}

		// Move From Hi register into rd
		void MfHi(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.hi.value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		void MtHi(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = registers.registerTable[rs].value;
		}

		void MtLo(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.lo.value = registers.registerTable[rs].value;
		}

		// Move from Lo Register into rd
		void MfLo(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.lo.value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
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

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
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

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// bitwise nor. rd = not(rs | rt)
		void Nor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ~(registers.registerTable[rs].value | registers.registerTable[rt].value);

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// bitwise xor. rd = rs ^ rt
		void Xor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value ^ registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// bitwise or. rd = rs | rt
		void Or(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value | registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// Set register if less than (signed)
		void Slt(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value < (int) registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// Set register if less than (unsigned)
		void SltU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value < (uint)registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// Shift Left Logical
		void SLL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value =  ((uint) registers.registerTable[rt].value) << shamt;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// shift left logical value
		void SLLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) << (registers.registerTable[rs].value);
		}

		// Shift Right Logical
		void SRL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value = ((uint) registers.registerTable[rt].value) >> shamt;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// shift right logical value
		void SRLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) >> (registers.registerTable[rs].value);
		}

		// Arithmetic Shift Right. Sign Extended.
		void SRA(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses signed numbers for arithmetic shifting.
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> shamt;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		void SRAV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> registers.registerTable[rs].value;
		}
		
		// Signed Subtraction. rd = rs - rt
		void Sub(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value - (int) registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		// Unsigned Subtraction. rd = rs - rt
		void SubU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value - (uint)registers.registerTable[rt].value;

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rd, registers.registerTable[rd].name, registers.registerTable[rd].value);
		}

		void MovZ(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value == 0)
				registers.registerTable[rd].value = registers.registerTable[rs].value;
		}

		void MovN(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value != 0)
				registers.registerTable[rd].value = registers.registerTable[rs].value;
		}

		void SysCall(byte rs, byte rt, byte rd, byte shamt)
		{
			var code = registers.registerTable[2].value; // get $v0 value

			/*
			 if(code == 1)
			 print_int
			 else if etc.
			 */
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// signed immediate addition
		void AddI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = (int) (registers.registerTable[rs].value + immediate);

			//Globals.parser.PrintToUI("Add " + registers.registerTable[rd].name + ", " + registers.registerTable[rs] + ", " + registers.registerTable[rt]);

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// unsigned immediate addition
		void AddIU(byte rs, byte rt, ushort immediate)
		{
			registers.registerTable[rt].value = (uint)(registers.registerTable[rs].value + immediate);

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Bitwise and immediate. 
		void AndI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value & immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Branch if Equal. 
		void Beq(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value == registers.registerTable[rt].value)
				Globals.AdvancePC((uint) (immediate << 2));
		}

		// Branch if Not Equal.
		void Bne(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value != registers.registerTable[rt].value)
				Globals.AdvancePC((uint)(immediate << 2));
		}

		void Blez(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value <= 0)
				Globals.PC = (uint)immediate;
		}

		void Bgtz(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value > 0)
				Globals.PC = (uint)immediate;
		}

		void XorI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value ^ immediate;
		}

		void Lb(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = Globals.staticData[registers.registerTable[rs].value + immediate];
		}

		void Lh(byte rs, byte rt, short immediate)
		{

		}

		// load byte immediate
		void Lbu(byte rs, byte rt, short immediate)
		{

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Load Halfword Unsigned
		void Lhu(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Load Upper Immediate
		void LuI(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Load Word
		void Lw(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Bitwise Or Immediate
		void OrI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value | immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sb(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sh(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Set on less than immediate (signed)
		void SltI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = ((int)registers.registerTable[rs].value) < immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Set on less than immediate (unsigned)
		void SltIU(byte rs, byte rt, ushort immediate)
		{
			registers.registerTable[rt].value = ((uint)registers.registerTable[rs].value) < immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sw(byte rs, byte rt, short immediate)
		{

			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// Jump command.
		void J(uint address)
		{
			Globals.PC = address;
			//Globals.PC = Globals.nPC;
			//Globals.nPC = (Globals.PC & 0xF0000000) | (address << 2);
		}

		// Jump and Link
		void Jal(uint address)
		{
			registers.registerTable[31].value = Globals.PC + 8; // r31 is $ra.
			Globals.PC = address;
			//Globals.PC = Globals.nPC;
			//Globals.nPC = (Globals.PC & 0xf0000000) | (address << 2);
		}
	}
}
