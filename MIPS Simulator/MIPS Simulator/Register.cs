using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS_Simulator
{
    class Register
    {

		private string _name, _alias; // name will follow Rx format and alias will follow MIPS format (t0, s0, etc)

		private bool _isUnsigned;

		dynamic _value;

		private int signedValue;
		private uint unsignedValue;

		public bool isUnsigned
		{
			get
			{
				return _isUnsigned; 
			}
			set
			{
				_isUnsigned = value;
			}
		}

		// initializes a new Register with a name and value
		public Register(string newName, string newAlias, int newValue)
		{
			_name = newName;
			_alias = newAlias;
			_value = 0;
			_isUnsigned = false;
		}

		// returns the name of the register
		public string name
		{
			get
			{
				return _name;
			}
		}

		// returns the MIPS name of the register.
		public string alias
		{
			get
			{
				return _alias;
			}
		}

		// public getter and setter for register value. Works for int and unsigned int
		public dynamic value
		{
			get
			{
				return _value;
			}
			set
			{
				if (value.GetType() == typeof(int) && value > int.MaxValue)
					// handle integer overflow
					_value = int.MinValue;
				else if (value.GetType() == typeof(int) && value < int.MinValue)
					// handle integer underflow
					_value = int.MaxValue;
				else if (value.GetType() == typeof(uint) && value > uint.MaxValue)
					// handle unsigned int overflow
					_value = uint.MinValue;
				else if (value.GetType() == typeof(uint) && value < uint.MinValue)
					// handle unsigned in underflow
					_value = uint.MaxValue;
				else
					_value = value; // value keyword is used for C# setters. If we had _value = 5, then value would be 5
			}
		}
    }
}
