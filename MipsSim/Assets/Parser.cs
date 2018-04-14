using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;

namespace MIPS_Simulator
{
	/// <summary>
	/// Parser will handle text file input. Other classes will use Parser to get op codes, rs codes, and more, which will
	/// determine the format of the op code (e.g: I, J, R-format).
	/// </summary>
    public class Parser
    {
		InstructionReader instRead;
		// contains every line of the text file
		private List<string> lines;

		// contains current index of 
		int currentIndex = 0;

		// Constructor will read in the filepath and store the text in the file to a list.
		public Parser(string filepath)
		{
			filepath = filepath.Replace("\n", ""); // get rid of illegal character from file path

			var temp = System.IO.File.ReadAllLines(filepath);
			lines = new List<string>(temp);

			instRead = new InstructionReader();
			foreach (string line in lines)
			{
				if (line.Contains("DATA SEGMENT")) // stop printing at data section
					break;

				instRead.ParseAndPrintInstruction(line);
			}

			MemoryInitializer.InitStaticData(lines);
			MemoryInitializer.InitStack();
			MemoryInitializer.InitTextData(lines);
		}

		// Get the next line in the text file
		public string GetLine()
		{
			string temp;

			if (currentIndex < lines.Count)
			{
				temp = lines[currentIndex];
				currentIndex++;
				return temp;
			}

			return null;
		}

		public void ExecuteLine()
		{
			instRead.ParseInstruction();
		}

		public void ExecuteProgram()
		{
			while(Globals.textData.ContainsKey((uint)Globals.PC))
			{ 
				// should execute program while PC remains valid
				instRead.ParseInstruction();
			}
		}

		void PrintToUI()
		{
			ScaleText scaler = GameObject.FindGameObjectWithTag("TextContent").GetComponent<ScaleText>();

			foreach (var line in lines)
				scaler.SetText(line);
		}

		public void PrintToUI(string newLine)
		{
			ScaleText scaler = GameObject.FindGameObjectWithTag("TextContent").GetComponent<ScaleText>();

			scaler.SetText(newLine + "\n");
		}

		// Get every line of the text file
		public List<string> GetLines()
		{
			return lines;
		}
    }
}
