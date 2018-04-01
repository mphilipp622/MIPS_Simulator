using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    class OperationManager
    {
		//private Dictionary<Tuple<byte, byte>, >

		///////////////////////////////////
		/////// R-Format Operations ///////
		///////////////////////////////////

		// signed Add
		public void Add(Register rs, Register rt, ref Register rd)
		{
			rd.value = (int) rs.value + (int) rt.value;
		}

		// unsigned add
		public void AddU(Register rs, Register rt, ref Register rd)
		{
			rd.value = (uint) rs.value + (uint) rt.value;
		}

		// bitwise and
		void And(Register rs, Register rt, ref Register rd)
		{
			rd.value = rs.value & rt.value;
		}

		// signed division
		void Div(Register rs, Register rt, ref Register hi, ref Register lo)
		{
			hi.value = (int) rs.value % (int) rt.value;
			lo.value = (int) rs.value / (int) rt.value;
		}

		// unsigned division
		void DivU(Register rs, Register rt, ref Register hi, ref Register lo)
		{
			hi.value = (uint) rs.value % (uint) rt.value;
			lo.value = (uint) rs.value / (uint) rt.value;
		}

		// Jump register. Stores rs value into program counter
		void JR(Register rs)
		{
			Globals.PC = rs.value;
		}

		// Move From Hi register into rd
		void MfHi(ref Register rd, Register hi)
		{
			rd.value = hi.value;
		}

		// Move from Lo Register into rd
		void MfLo(ref Register rd, Register lo)
		{
			rd.value = lo.value;
		}

		// signed multiplication. Stores significant 32-bits into Hi and lower 32-bits into Lo
		void Mult(Register rs, Register rt, ref Register hi, ref Register lo)
		{
			long product = (int) rs.value * (int) rt.value;
			var newHi = (int) (product >> 32); // shift top 32 to lowest 32.
			var newLo = (int) (product << 32); // kill leading 32 bits
			newLo = (int)(product >> 32); // shift back to lower 32-bits.

			hi.value = newHi;
			lo.value = newLo;
		}

		// unsigned multiplication. Stores sig 32-bits into Hi and low 32-bits into Lo
		void Multu(Register rs, Register rt, ref Register hi, ref Register lo)
		{
			ulong product = (uint)rs.value * (uint)rt.value;
			var newHi = (uint) (product >> 32);
			var newLo = (uint)(product << 32); // kill leading 0's
			newLo = (uint)(product >> 32);

			hi.value = newHi;
			lo.value = newLo;
		}

		// bitwise nor. rd = not(rs | rt)
		void Nor(Register rs, Register rt, ref Register rd)
		{
			rd.value = ~(rs.value | rt.value);
		}

		// bitwise xor. rd = rs ^ rt
		void Xor(Register rs, Register rt, ref Register rd)
		{
			rd.value = rs.value ^ rt.value;
		}

		// bitwise or. rd = rs | rt
		void Or(Register rs, Register rt, ref Register rd)
		{
			rd.value = rs.value | rt.value;
		}

		// Set register if less than (signed)
		void Slt(Register rs, Register rt, ref Register rd)
		{
			rd.value = (int) rs.value < (int) rt.value;
		}

		// Set register if less than (unsigned)
		void SltU(Register rs, Register rt, ref Register rd)
		{
			rd.value = (uint)rs.value < (uint)rt.value;
		}

		// Shift Left Logical
		void SLL(Register rt, ref Register rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			rd.value =  ((uint) rt.value) << shamt;
		}

		// Shift Right Logical
		void SRL(Register rt, ref Register rd, byte shamt)
		{
			// C# uses unsigned numbers for logical shifting
			rd.value = ((uint) rt.value) >> shamt;
		}

		// Arithmetic Shift Right. Sign Extended.
		void SRA(Register rt, ref Register rd, byte shamt)
		{
			// C# uses signed numbers for arithmetic shifting.
			rd.value = ((int)rt.value) >> shamt;
		}

		// Signed Subtraction. rd = rs - rt
		void Sub(Register rs, Register rt, ref Register rd)
		{
			rd.value = (int) rs.value - (int) rt.value;
		}

		// Unsigned Subtraction. rd = rs - rt
		void SubU(Register rs, Register rt, ref Register rd)
		{
			rd.value = (uint)rs.value - (uint)rt.value;
		}

		///////////////////////////////////
		/////// I-Format Operations ///////
		///////////////////////////////////

		// signed immediate addition
		void AddI(Register rs, ref Register rt, short immediate)
		{
			rt.value = (int) (rs.value + immediate);
		}

		// unsigned immediate addition
		void AddIU(Register rs, ref Register rt, ushort immediate)
		{
			rt.value = (uint)(rs.value + immediate);
		}

		// Bitwise and immediate. 
		void AndI(Register rs, ref Register rt, short immediate)
		{
			rt.value = rs.value & immediate;
		}

		// Branch if Equal. 
		void Beq(Register rs, Register rt, short immediate)
		{
			if (rs.value == rt.value)
				Globals.PC = Globals.PC + (uint) (immediate << 2);
			else
				Globals.PC = Globals.PC + 4;
		}

		// Branch if Not Equal.
		void Bne(Register rs, Register rt, short immediate)
		{
			if (rs.value != rt.value)
				Globals.PC = Globals.PC + (uint)(immediate << 2);
			else
				Globals.PC = Globals.PC + 4;
		}

		// load byte immediate
		void Lbu(Register rs, ref Register rd, short immediate)
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
		void OrI()
		{

		}

		void Sb()
		{

		}

		void Sh()
		{

		}

		void SltI()
		{

		}
		
		void SltIU()
		{

		}

		void Sw()
		{

		}
	}
}
