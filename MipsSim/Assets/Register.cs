using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MIPS_Simulator
{
    public class Register
    {

		private string _name, _alias; // name will follow Rx format and alias will follow MIPS format (t0, s0, etc)

		private bool _isUnsigned;

		dynamic _value;

		// initializes a new Register with a name and value
		public Register(string newName, string newAlias, int newValue)
		{
			_name = newName;
			_alias = newAlias;
			_value = newValue;
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
				if (alias == "$sp")
				{
					// execute memory allocation and deallocation for the stack
					int difference = value - _value; // find out if we're pushing or popping

					if (difference < 0)
					{
						// pushing memory onto stack
						for (uint i = 4; i <= -difference; i += 4)
							Globals.stackData.Add((uint)(_value - i), 0); // allocate stack memory
					}
					else if (difference > 0)
					{
						// popping memory from stack
						for (int i = 0; i < difference; i += 4)
							Globals.stackData.Remove((uint)(_value + i));
					}

					_value = value;
				}
				else if (_alias == "$zero")
					_value = 0;
				else
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

				if(_alias != "$hi" && _alias != "$lo")
					RegisterTextManager.instance.SetRegisterText(Convert.ToByte(name.Trim('R')), alias, _value); // update register value in UI when value is set
				else if(_alias == "$hi")
					RegisterTextManager.instance.SetRegisterText(32, alias, _value);
				else if(_alias == "$lo")
					RegisterTextManager.instance.SetRegisterText(33, alias, _value);
			}
		}
    }
}
