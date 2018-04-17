using System;
using System.Collections.Generic;
using UnityEngine;

namespace MIPS_Simulator
{
	// Basically decodes all the operations
	class OperationManager
    {
		public static RegisterManager registers; // stores registers into hash tables. Called on to access registers. Hate to make it static though.
		//private RegisterRegisterTextManager.instanceager RegisterTextManager.instance;

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
			//RegisterTextManager.instance = GameObject.FindGameObjectWithTag("RegisterRegisterTextManager.instanceager").GetComponent<RegisterRegisterTextManager.instanceager>();

			// Set all the text to initial values
			foreach (KeyValuePair<byte, Register> r in registers.registerTable)
				RegisterTextManager.instance.SetRegisterText(r.Key, r.Value.alias, r.Value.value);

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

			//if (opCode == 11 || opCode == 36 || opCode == 37)
			//{
			//	iFormatOpsU[opCode](rs, rt, (ushort)immediate);
			//}
			//else
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
			
			iFormatOps.Add(4, new iFunc(Beq));
			iFormatOps.Add(5, new iFunc(Bne));
			iFormatOps.Add(6, new iFunc(Blez));
			iFormatOps.Add(7, new iFunc(Bgtz));
			iFormatOps.Add(9, new iFunc(AddIU));
			iFormatOps.Add(11, new iFunc(SltIU));
			
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

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// unsigned add
		public void AddU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value + (uint) registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise and
		void And(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value & registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// signed division
		void Div(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (int)registers.registerTable[rs].value % (int) registers.registerTable[rt].value;
			Globals.lo.value = (int)registers.registerTable[rs].value / (int) registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			//RegisterTextManager.instance.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// unsigned division
		void DivU(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = (uint)registers.registerTable[rs].value % (uint) registers.registerTable[rt].value;
			Globals.lo.value = (uint)registers.registerTable[rs].value / (uint) registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			//RegisterTextManager.instance.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// Jump register. Stores rs value into program counter
		void JR(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.PC = Convert.ToInt32(registers.registerTable[rs].value);

			RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
		}

		// jump and link register
		void JALR(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[31].value = Globals.PC + 4;
			Globals.PC = Convert.ToInt32(registers.registerTable[rs].value);

			//RegisterTextManager.instance.SetRegisterText(31, "$ra", registers.registerTable[31].value);
			RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
		}

		// Move From Hi register into rd
		void MfHi(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.hi.value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void MtHi(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.hi.value = registers.registerTable[rs].value;

			//RegisterTextManager.instance.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
		}

		void MtLo(byte rs, byte rt, byte rd, byte shamt)
		{
			Globals.lo.value = registers.registerTable[rs].value;

			//RegisterTextManager.instance.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// Move from Lo Register into rd
		void MfLo(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = Globals.lo.value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
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

			//RegisterTextManager.instance.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			//RegisterTextManager.instance.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
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

			//RegisterTextManager.instance.SetRegisterText(32, Globals.hi.alias, Globals.hi.value);
			//RegisterTextManager.instance.SetRegisterText(33, Globals.lo.alias, Globals.lo.value);
		}

		// bitwise nor. rd = not(rs | rt)
		void Nor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ~(registers.registerTable[rs].value | registers.registerTable[rt].value);

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise xor. rd = rs ^ rt
		void Xor(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value ^ registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// bitwise or. rd = rs | rt
		void Or(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = registers.registerTable[rs].value | registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Set register if less than (signed)
		void Slt(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value < (int) registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Set register if less than (unsigned)
		void SltU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value < (uint)registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Shift Left Logical
		void SLL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value =  ((uint) registers.registerTable[rt].value) << shamt;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// shift left logical value
		void SLLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) << (registers.registerTable[rs].value);

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Shift Right Logical
		void SRL(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			registers.registerTable[rd].value = ((uint) registers.registerTable[rt].value) >> shamt;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// shift right logical value
		void SRLV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((uint)registers.registerTable[rt].value) >> (registers.registerTable[rs].value);

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Arithmetic Shift Right. Sign Extended.
		void SRA(byte rs, byte rt, byte rd, byte shamt)
		{
			// C# uses signed numbers for arithmetic shifting.
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> shamt;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void SRAV(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = ((int)registers.registerTable[rt].value) >> registers.registerTable[rs].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Signed Subtraction. rd = rs - rt
		void Sub(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (int)registers.registerTable[rs].value - (int) registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		// Unsigned Subtraction. rd = rs - rt
		void SubU(byte rs, byte rt, byte rd, byte shamt)
		{
			registers.registerTable[rd].value = (uint)registers.registerTable[rs].value - (uint)registers.registerTable[rt].value;

			//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
		}

		void MovZ(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value == 0)
			{
				registers.registerTable[rd].value = registers.registerTable[rs].value;
				//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
			}
		}

		void MovN(byte rs, byte rt, byte rd, byte shamt)
		{
			if (registers.registerTable[rt].value != 0)
			{
				registers.registerTable[rd].value = registers.registerTable[rs].value;
				//RegisterTextManager.instance.SetRegisterText(rd, registers.registerTable[rd].alias, registers.registerTable[rd].value);
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
				RegisterTextManager.instance.SetRegisterText(2, registers.registerTable[2].alias, registers.registerTable[2].value); // Set $v0 text
			}
			else if (code == 8)
				UIManager.instance.ReadString();
			else if (code == 10)
				Application.Quit(); // kill application
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// signed immediate addition
		void AddI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = (int) (registers.registerTable[rs].value + immediate);

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// unsigned immediate addition
		void AddIU(byte rs, byte rt, short immediate)
		{
			int result = Convert.ToInt32(registers.registerTable[rs].value + immediate);
			registers.registerTable[rt].value = result;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Bitwise and immediate. 
		void AndI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value & immediate;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Branch if Equal. 
		void Beq(byte rs, byte rt, short immediate)
		{
			if (Convert.ToInt32(registers.registerTable[rs].value) == Convert.ToInt32(registers.registerTable[rt].value))
			{
				//Globals.AdvancePC((uint) (immediate << 2));
				int offset = Convert.ToInt32(immediate);
				offset = (offset << 2) - 4;

				Globals.PC += offset;
				RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		// Branch if Not Equal.
		void Bne(byte rs, byte rt, short immediate)
		{
			if (Convert.ToInt32(registers.registerTable[rs].value) != Convert.ToInt32(registers.registerTable[rt].value))
			{
				//Globals.AdvancePC((uint)(immediate << 2));
				int offset = Convert.ToInt32(immediate);
				offset = (offset << 2) - 4;

				Globals.PC += offset;
				//Debug.Log("Branch to Address    " + Globals.PC.ToString("X"));

				RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void Blez(byte rs, byte rt, short immediate)
		{
			if (Convert.ToInt32(registers.registerTable[rs].value) <= 0)
			{
				int offset = Convert.ToInt32(immediate);
				offset = (offset << 2) - 4;

				Globals.PC += offset;
				RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void Bgtz(byte rs, byte rt, short immediate)
		{
			if (Convert.ToInt32(registers.registerTable[rs].value) > 0)
			{
				int offset = Convert.ToInt32(immediate);
				offset = (offset << 2) - 4;

				Globals.PC += offset;
				RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
			}
		}

		void XorI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value ^ immediate;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Lb(byte rs, byte rt, short immediate)
		{
			Byte[] tempBytes = null;
			uint offset = Convert.ToUInt32(registers.registerTable[rs].value);

			Byte[] destBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(destBytes);

			if (registers.registerTable[rs].alias == "$sp")
			{
				// load from stack
				tempBytes = BitConverter.GetBytes(Globals.stackData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}
			else
			{
				tempBytes = BitConverter.GetBytes(Globals.staticData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}

			destBytes[0] = tempBytes[immediate]; // load halfword at immediate + 0 and immediate + 1 into destination

			registers.registerTable[rt].value = BitConverter.ToInt32(destBytes, 0);

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// load byte unsigned
		void Lbu(byte rs, byte rt, short immediate)
		{
			Byte[] tempBytes = null;
			uint offset = Convert.ToUInt32(registers.registerTable[rs].value);

			Byte[] destBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(destBytes);

			if (registers.registerTable[rs].alias == "$sp")
			{
				// load from stack
				tempBytes = BitConverter.GetBytes(Globals.stackData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}
			else
			{
				tempBytes = BitConverter.GetBytes(Globals.staticData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}

			destBytes[0] = tempBytes[immediate]; // load halfword at immediate + 0 and immediate + 1 into destination

			registers.registerTable[rt].value = BitConverter.ToUInt32(destBytes, 0);

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Lh(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return; // must be multiple of two

			Byte[] tempBytes = null;
			uint offset = Convert.ToUInt32(registers.registerTable[rs].value);

			Byte[] destBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(destBytes);

			if (registers.registerTable[rs].alias == "$sp")
			{
				// load from stack
				tempBytes = BitConverter.GetBytes(Globals.stackData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}
			else
			{
				tempBytes = BitConverter.GetBytes(Globals.staticData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}

			for (int i = 0; i < 2; i++)
				destBytes[i] = tempBytes[immediate + i]; // load halfword at immediate + 0 and immediate + 1 into destination

			registers.registerTable[rt].value = BitConverter.ToInt32(destBytes, 0);

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Halfword Unsigned
		void Lhu(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return; // must be multiple of two

			Byte[] tempBytes = null;
			uint offset = Convert.ToUInt32(registers.registerTable[rs].value);

			Byte[] destBytes = BitConverter.GetBytes(registers.registerTable[rt].value);
			Array.Reverse(destBytes);

			if (registers.registerTable[rs].alias == "$sp")
			{
				// load from stack
				tempBytes = BitConverter.GetBytes(Globals.stackData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}
			else
			{
				tempBytes = BitConverter.GetBytes(Globals.staticData[offset]); // get stack data's bytes
				Array.Reverse(tempBytes); // switch to big endian
			}

			for (int i = 0; i < 2; i++)
				destBytes[i] = tempBytes[immediate + i]; // load halfword at immediate + 0 and immediate + 1 into destination

			registers.registerTable[rt].value = BitConverter.ToUInt32(destBytes, 0);

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Word
		void Lw(byte rs, byte rt, short immediate)
		{
			if (immediate % 4 != 0)
				return; // must be multiple of two

			uint offset = Convert.ToUInt32(registers.registerTable[rs].value + immediate);

			//Debug.Log("LW " + registers.registerTable[rt].alias + ", " + immediate.ToString("X") + "(" + registers.registerTable[rs].alias + ")");

			if (registers.registerTable[rs].alias == "$sp")
				// load from stack
				registers.registerTable[rt].value = Globals.stackData[offset];
			else
				registers.registerTable[rt].value = Globals.staticData[offset];

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Load Upper Immediate
		void LuI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = immediate << 16;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Bitwise Or Immediate
		void OrI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = registers.registerTable[rs].value | immediate;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		void Sb(byte rs, byte rt, short immediate)
		{
			// get the byte from our register
			Byte[] tempBytes = BitConverter.GetBytes(registers.registerTable[rt].value); // get new value's bytes
			Array.Reverse(tempBytes); // switch to big endian

			Byte[] destBytes = null;

			if (registers.registerTable[rs].alias == "$sp") // store to stack, not static
			{
				destBytes = BitConverter.GetBytes(Globals.stackData[(uint)registers.registerTable[rs].value]); // get bytes from destination value
				Array.Reverse(destBytes); // switch them to big endian

				destBytes[immediate] = tempBytes[0]; // write least sig values from rt into stack

				Globals.stackData[(uint)registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0); // rewrite value into stack

				UIManager.instance.WriteStackMemory((uint)registers.registerTable[rs].value, BitConverter.ToInt32(destBytes, 0));
			}
			else
			{
				// otherwise, we store onto static data segment.
				destBytes = BitConverter.GetBytes(Globals.staticData[(uint)registers.registerTable[rs].value]); // get bytes from destination value
				Array.Reverse(destBytes); // switch them to big endian

				destBytes[immediate] = tempBytes[0]; // write least sig values from rt into stack

				Globals.staticData[(uint)registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0);

				UIManager.instance.WriteStaticMemory((uint)registers.registerTable[rs].value, BitConverter.ToInt32(destBytes, 0));
			}

			

		}

		void Sh(byte rs, byte rt, short immediate)
		{
			if (immediate % 2 != 0)
				return;

			Byte[] tempBytes = BitConverter.GetBytes(registers.registerTable[rt].value); // get new value's bytes
			Array.Reverse(tempBytes); // switch to big endian

			Byte[] destBytes = null;

			if (registers.registerTable[rs].alias == "$sp") // store to stack, not static
			{
				destBytes = BitConverter.GetBytes(Globals.stackData[(uint)registers.registerTable[rs].value]); // get bytes from destination value
				Array.Reverse(destBytes); // switch them to big endian

				for (int i = 0; i < 2; i++)
					destBytes[immediate + i] = tempBytes[i]; // write least sig values from rt into stack

				Globals.stackData[(uint)registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0); // rewrite value into stack

				UIManager.instance.WriteStackMemory((uint)registers.registerTable[rs].value, BitConverter.ToInt32(destBytes, 0));
			}
			else
			{
				// otherwise, we store onto static data segment.
				destBytes = BitConverter.GetBytes(Globals.staticData[(uint)registers.registerTable[rs].value]); // get bytes from destination value
				Array.Reverse(destBytes); // switch them to big endian

				for (int i = 0; i < 2; i++)
					destBytes[immediate + i] = tempBytes[i]; // write least sig values from rt into stack

				Globals.staticData[(uint)registers.registerTable[rs].value] = BitConverter.ToInt32(destBytes, 0);

				UIManager.instance.WriteStaticMemory((uint)registers.registerTable[rs].value, BitConverter.ToInt32(destBytes, 0));
			}
			
			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		void Sw(byte rs, byte rt, short immediate)
		{
			if (immediate % 4 != 0)
				return;

			uint offset = Convert.ToUInt32(registers.registerTable[rs].value + immediate);

			if (registers.registerTable[rs].alias == "$sp")
			{
				Globals.stackData[offset] = registers.registerTable[rt].value;

				UIManager.instance.WriteStackMemory(offset, registers.registerTable[rt].value);
			}
			else
			{
				Globals.staticData[offset] = registers.registerTable[rt].value;

				UIManager.instance.WriteStaticMemory(offset, registers.registerTable[rt].value);
			}

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].name, registers.registerTable[rt].value);
		}

		// Set on less than immediate (signed)
		void SltI(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = ((int)registers.registerTable[rs].value) < immediate;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		// Set on less than immediate (unsigned)
		void SltIU(byte rs, byte rt, short immediate)
		{
			registers.registerTable[rt].value = ((uint)registers.registerTable[rs].value) < immediate;

			//RegisterTextManager.instance.SetRegisterText(rt, registers.registerTable[rt].alias, registers.registerTable[rt].value);
		}

		

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// Jump command.
		void J(uint address)
		{
			int offset = Convert.ToInt32(address);
			offset = (offset << 2);

			Globals.PC = offset;
			//Globals.PC = Globals.nPC;
			//Globals.nPC = (Globals.PC & 0xF0000000) | (address << 2);
			RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
		}

		// Jump and Link
		void Jal(uint address)
		{
			registers.registerTable[31].value = Globals.PC + 8; // r31 is $ra.

			int offset = Convert.ToInt32(address);
			offset = (offset << 2);

			//Debug.Log(offset.ToString("X"));
			Globals.PC = offset;
			//int offset = Convert.ToInt32(address << 2);

			//int topBits = (int)(Globals.PC & 0xF0000000);
			//Debug.Log(topBits.ToString("X") + " | " + offset.ToString("X") + " = " + (topBits | offset).ToString("X"));
			//Globals.PC = (topBits | offset) - 36;
			//Globals.PC = Globals.nPC;
			//Globals.nPC = (Globals.PC & 0xf0000000) | (address << 2);
			//RegisterTextManager.instance.SetRegisterText(31, "ra", registers.registerTable[31].value);
			RegisterTextManager.instance.SetRegisterText(34, "$PC", Globals.PC);
		}
	}
}
