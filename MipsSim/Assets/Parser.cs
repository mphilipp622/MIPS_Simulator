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

			ParseDataSection();
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

		// finds the .data section and will tell Globals.staticData to initialize with that data.
		void ParseDataSection()
		{
			// Find the line where .data is declared
			int index = 0;
			foreach(string line in lines)
			{
				if (line.Contains("DATA SEGMENT"))
					break;
				index++;
			}

			Globals.staticData = new Dictionary<uint, int>();

			uint memIndex = 0x10010000;

			for (int i = index + 1; i < lines.Count; i++)
			{
				string[] values = lines[i].Split(' '); // grab each hex value in the line

				int data = Convert.ToInt32(values[1], 16); // get data value as hex

				Globals.staticData.Add(memIndex, data);
				//Debug.Log("[" + memIndex.ToString("X") + "]    " + Globals.staticData[memIndex].ToString("X"));
				memIndex += 4;
			}

		}

		public void ExecuteLine()
		{
			instRead.ParseInstruction(GetLine());
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
