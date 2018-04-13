using System;
using System.Collections.Generic;
using UnityEngine;

namespace MIPS_Simulator
{
	// Basically decodes all the operations
	class OperationManager
    {
		public static RegisterManager registers; // stores registers into hash tables. Called on to access registers. Hate to make it static though.
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
			textMan = GameObject.FindGameObjectWithTag("RegisterTextManager").GetComponent<RegisterTextManager>();

			// Set all the text to initial values
			foreach (KeyValuePair<byte, Register> r in registers.registerTable)
				textMan.SetRegisterText(r.Key, r.Value.alias, r.Value.value);

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

			if (opCode == 11 || opCode == 36 || opCode == 37)
			{
				iFormatOpsU[opCode](rs, rt, (ushort)immediate);
			}
			else
				iFormatOps[opCode](rs, rt, (short)immediate);
		}

		public void ExecuteJFormatOp(byte opCode, uint newAddress)
		{
			jFormatOps[opCode](newAddress);
		}

		// Initializes and populates R-Format Dictionary
		private void InitRFormat()
		{
			rFormatOps = new Dictionary<Tuple<byte, byte>, rFunc>();

			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x20), new rFunc(Add));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x21), new rFunc(AddU));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x24), new rFunc(And));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x1A), new rFunc(Div));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x1B), new rFunc(DivU));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x08), new rFunc(JR));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x09), new rFunc(JALR));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x10), new rFunc(MfHi));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x11), new rFunc(MtHi));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x12), new rFunc(MfLo));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x13), new rFunc(MtLo));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x18), new rFunc(Mult));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x19), new rFunc(Multu));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x27), new rFunc(Nor));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x26), new rFunc(Xor));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x25), new rFunc(Or));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2A), new rFunc(Slt));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x2B), new rFunc(SltU));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x00), new rFunc(SLL));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x04), new rFunc(SLLV));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x02), new rFunc(SRL));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x06), new rFunc(SRLV));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x03), new rFunc(SRA));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x07), new rFunc(SRAV));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x22), new rFunc(Sub));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0x23), new rFunc(SubU));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xA), new rFunc(MovZ));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xB), new rFunc(MovN));
			rFormatOps.Add(new Tuple<byte, byte>(0x00, 0xC), new rFunc(SysCall));
		}

		// Initializes and populates I-Format Dictionary
		private void InitIFormat()
		{
			iFormatOps = new Dictionary<byte, iFunc>();
			iFormatOpsU = new Dictionary<byte, iFuncU>();

			// populate signed function hashtable
			
			iFormatOps.Add(8, new iFunc(AddI));
			iFormatOps.Add(10, new iFunc(SltI));
			iFormatOps.Add(12, new iFunc(AndI));
			iFormatOps.Add(13, new iFunc(OrI));
			iFormatOps.Add(14, new iFunc(XorI));
			iFormatOps.Add(15, new iFunc(LuI));
			iFormatOps.Add(32, new iFunc(Lb));
			iFormatOps.Add(33, new iFunc(Lh));
			iFormatOps.Add(35, new iFunc(Lw));
			iFormatOps.Add(36, new iFunc(Lbu));
			iFormatOps.Add(37, new iFunc(Lhu));
			iFormatOps.Add(40, new iFunc(Sb));
			iFormatOps.Add(41, new iFunc(Sh));
			iFormatOps.Add(43, new iFunc(Sw));
			
			iFormatOpsU.Add(4, new iFuncU(Beq));
			iFormatOpsU.Add(5, new iFuncU(Bne));
			iFormatOpsU.Add(6, new iFuncU(Blez));
			iFormatOpsU.Add(7, new iFuncU(Bgtz));
			iFormatOps.Add(9, new iFunc(AddIU));
			iFormatOpsU.Add(11, new iFuncU(SltIU));
			
		}

		// Initializes and Populates J-Format Dictionary
		private void InitJFormat()
		{
			jFormatOps = new Dictionary<byte, jFunc>();

			jFormatOps.Add(2, new jFunc(J));
			jFormatOps.Add(3, new jFunc(Jal));
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

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// unsigned add
		public void AddU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value + (uint) registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise and
		void And(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value & registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// signed division
		void Div(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (int)registers.registerTable[rs].value % (int) registers.registerTable[rt].value;
			Globals.lo.value = (int)registers.registerTable[rs].value / (int) registers.registerTable[rt].value;

			textMan.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			textMan.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// unsigned division
		void DivU(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (uint)registers.registerTable[rs].value % (uint) registers.registerTable[rt].value;
			Globals.lo.value = (uint)registers.registerTable[rs].value / (uint) registers.registerTable[rt].value;

			textMan.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			textMan.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// Jump register. Stores rs value into program counter
		void JR(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.PC = registers.registerTable[rs].value;

			textMan.SetRegisterText(34, "$PC", Globals.PC);
		}

		// jump and link register
		void JALR(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[31].value = Globals.PC + 4;
			Globals.PC = registers.registerTable[rs].value;

			textMan.SetRegisterText(31, "$ra", registers.registerTable[31].value);
			textMan.SetRegisterText(34, "$PC", Globals.PC);
		}

		// Move From Hi register into rd
		void MfHi(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.hi.value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void MtHi(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = registers.registerTable[rs].value;

			textMan.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
		}

		void MtLo(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.lo.value = registers.registerTable[rs].value;

			textMan.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// Move from Lo Register into rd
		void MfLo(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.lo.value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
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

			textMan.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			textMan.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
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

			textMan.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			textMan.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// bitwise nor. rd = not(rs | rt)
		void Nor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ~(registers.registerTable[rs].value | registers.registerTable[rt].value);

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise xor. rd = rs ^ rt
		void Xor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value ^ registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise or. rd = rs | rt
		void Or(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value | registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Set register if less than (signed)
		void Slt(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value < (int) registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Set register if less than (unsigned)
		void SltU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value < (uint)registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Shift Left Logical
		void SLL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value =  ((uint) registers.registerTable[rt].value) << shamt;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// shift left logical value
		void SLLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) << (registers.registerTable[rs].value);

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Shift Right Logical
		void SRL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value = ((uint) registers.registerTable[rt].value) >> shamt;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// shift right logical value
		void SRLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) >> (registers.registerTable[rs].value);

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Arithmetic Shift Right. Sign Extended.
		void SRA(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses signed numbers for arithmetic shifting.
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> shamt;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void SRAV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> registers.registerTable[rs].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Signed Subtraction. rd = rs - rt
		void Sub(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value - (int) registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Unsigned Subtraction. rd = rs - rt
		void SubU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value - (uint)registers.registerTable[rt].value;

			textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void MovZ(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value == 0)
			{
				registers.registerTable[rd].value = registers.registerTable[rs].value;
				textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
			}
		}

		void MovN(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value != 0)
			{
				registers.registerTable[rd].value = registers.registerTable[rs].value;
				textMan.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
			}
		}

		void SysCall(byte rs, byte rt, byte rd, byte shamt)
		{
			var code = registers.registerTable[2].value; // get $v0 value

			if (code == 1)
				UIManager.instance.PrintInt(registers.registerTable[4].value);
			else if (code == 4)
				UIManager.instance.PrintString(registers.registerTable[4].value);
			else if (code == 5)
			{
				UIManager.instance.ReadInt(); // STILL NEED TO STORE  INT INTO V0
				textMan.SetRegisterText(2, registers.registerTable[2].alias, registers.registerTable[2].value); // Set $v0 text
			}
			else if (code == 8)
				UIManager.instance.ReadString();
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// signed immediate addition
		void AddI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = (int) (registers.registerTable[rs].value + immediate);

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// unsigned immediate addition
		void AddIU(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = (uint)(registers.registerTable[rs].value + immediate);

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Bitwise and immediate. 
		void AndI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value & immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Branch if Equal. 
		void Beq(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value == registers.registerTable[rt].value)
			{
				//Globals.AdvancePC((uint) (immediate << 2));
				Globals.PC = (uint)immediate;
				textMan.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		// Branch if Not Equal.
		void Bne(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value != registers.registerTable[rt].value)
			{
				//Globals.AdvancePC((uint)(immediate << 2));
				Globals.PC = (uint)immediate;
				textMan.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void Blez(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value <= 0)
			{
				Globals.PC = (uint)immediate;
				textMan.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void Bgtz(byte rs, byte rt, ushort immediate)
		{
			if (registers.registerTable[rs].value > 0)
			{
				Globals.PC = (uint)immediate;
				textMan.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void XorI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value ^ immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Lb(byte rs, byte rt, short immediate)
		{
			Byte[] bytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(bytes); // swap to big endian

			registers.registerTable[rt].value = (int) bytes[immediate];

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// load byte unsigned
		void Lbu(byte rs, byte rt, short immediate)
		{
			Byte[] bytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(bytes); // swap to big endian

			registers.registerTable[rt].value = (uint) bytes[immediate];

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Lh(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return; // must be multiple of two

			Byte[] bytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(bytes); // swap to big endian

			Byte[] tempBytes = { bytes[immediate], bytes[immediate + 1] };

			short tempVal = BitConverter.ToInt16(tempBytes, 0);

			registers.registerTable[rt].value = (int) tempVal;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Halfword Unsigned
		void Lhu(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return; // must be multiple of two

			Byte[] bytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(bytes); // swap to big endian

			Byte[] tempBytes = { bytes[immediate], bytes[immediate + 1] };

			ushort tempVal = BitConverter.ToUInt16(tempBytes, 0);

			registers.registerTable[rt].value = (uint)tempVal;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Word
		void Lw(byte rs, byte rt, short immediate)
		{
			if (immediate % 4 != 0)
				return; // must be multiple of two

			registers.registerTable[rt].value = Globals.staticData[registers.registerTable[rs].value + (int)immediate];

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Upper Immediate
		void LuI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = immediate << 16;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Bitwise Or Immediate
		void OrI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value | immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Sb(byte rs, byte rt, short immediate)
		{
			Byte[] tempBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(tempBytes); // switch to big endian

			Byte[] destBytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(destBytes);

			destBytes[immediate] = tempBytes[0]; // replace destination byte at offset with the lowest byte of $rt

			Globals.staticData[registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0);
			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sh(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return;

			Byte[] tempBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(tempBytes); // switch to big endian

			Byte[] destBytes = BitConverter.GetBytes(Globals.staticData[registers.registerTable[rs].value]);
			Array.Reverse(destBytes);

			destBytes[immediate] = tempBytes[0]; // replace destination byte at offset with the lowest byte of $rt
			destBytes[immediate + 1] = tempBytes[1]; // replace destination at offset + 1 with second lowest byte

			Globals.staticData[registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0);
			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sw(byte rs, byte rt, short immediate)
		{
			if (immediate % 4 != 0)
				return;

			Globals.staticData[registers.registerTable[rs].value + (uint)immediate] = (int) registers.registerTable[rt].value;
			//textMan.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Set on less than immediate (signed)
		void SltI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = ((int)registers.registerTable[rs].value) < immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Set on less than immediate (unsigned)
		void SltIU(byte rs, byte rt, ushort immediate)
		{
			registers.registerTable[rt].value = ((uint)registers.registerTable[rs].value) < immediate;

			textMan.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
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
			textMan.SetRegisterText(34, "$PC", Globals.PC);
		}

		// Jump and Link
		void Jal(uint address)
		{
			registers.registerTable[31].value = Globals.PC + 8; // r31 is $ra.
			Globals.PC = address;
			//Globals.PC = Globals.nPC;
			//Globals.nPC = (Globals.PC & 0xf0000000) | (address << 2);
			textMan.SetRegisterText(31, "ra", registers.registerTable[31].value);
			textMan.SetRegisterText(34, "$PC", Globals.PC);
		}
	}
}
