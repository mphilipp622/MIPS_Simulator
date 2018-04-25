using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MIPS_Simulator
{
    public static class Globals
    {
		// filepath to the .s MIPS assembly file.
		public static string filePath;
		
		// program counter
		private static int _PC;

		public static int PC
		{
			get
			{
				return _PC;
			}
			set
			{
				//Image highlighter = GameObject.FindGameObjectWithTag("Highlighter").GetComponent<Image>();

				//int numberOfSteps = Globals.PC > 0 ? (value % _PC) / 4 : 0; // find out how far highlighter needs to jump

				//if (value < _PC)
				//	highlighter.rectTransform.anchoredPosition += new Vector2(0, (numberOfSteps * 45f));
				//else
				//	highlighter.rectTransform.anchoredPosition -= new Vector2(0, (numberOfSteps * 45f));

				_PC = value;

				UIManager.instance.SetHighlightedText(_PC);

				RegisterTextManager.instance.SetRegisterText(34, "$PC", _PC);
			}
		}

		// next program counter
		public static int nPC;

		public static dynamic intToDisplay;

		// text parser
		public static Parser parser;

		// Global accessors to registers. Mostly used by UI
		//public static RegisterManager RM = new RegisterManager();
		public static Register hi = new Register("hi", "$hi", 0);
		public static Register lo = new Register("lo", "$lo", 0);

		// Function for incrementing PC and nPC
		public static void AdvancePC(int offset)
		{
			PC = nPC;
			nPC += offset;
		}

		// Memory Allocation Dictionaries. Memory address will be used for key, instruction for value
		public static Dictionary<uint, int> textData;
		public static Dictionary<uint, int> staticData; // key will be base memory address. value will be a byte of the data
														
		public static Dictionary<uint, int> stackData;
	}
}
