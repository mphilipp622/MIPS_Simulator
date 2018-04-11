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

			//PrintToUI();
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

		//void PrintToUI()
		//{
		//	ScaleText scaler = GameObject.FindGameObjectWithTag("TextContent").GetComponent<ScaleText>();

		//	foreach (var line in lines)
		//		scaler.SetText(line);
		//}

		public void PrintToUI(string newLine)
		{
			ScaleText scaler = GameObject.FindGameObjectWithTag("TextContent").GetComponent<ScaleText>();

			scaler.SetText(newLine);
		}

		// Get every line of the text file
		public List<string> GetLines()
		{
			return lines;
		}
    }
}
