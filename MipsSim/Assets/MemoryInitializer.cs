﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIPS_Simulator;
using UnityEngine;

public static class MemoryInitializer
{
	public static void InitStack()
	{
		Globals.stackData = new Dictionary<uint, int>();
		Globals.stackData.Add(0x7fffeffc, 0);
	}

	public static void InitStaticData(List<string> lines)
	{
		// Find the line where .data is declared
		int index = 0;
		foreach (string line in lines)
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

			//Byte[] bytes = BitConverter.GetBytes(data);
			//Array.Reverse(bytes); // swap to big endian

			//Globals.staticData.Add(memIndex, new List<byte>());

			//foreach (Byte b in bytes)
			//{
			//	Globals.staticData[memIndex].Add(b); // add each byte into the list at this memory address
			//	Debug.Log(memIndex.ToString("X") + "    " + Convert.ToChar(b));
			//}

			//Debug.Log("[" + memIndex.ToString("X") + "]    " + Globals.staticData[memIndex].ToString("X"));
			memIndex += 4;
		}
	}

	public static void InitTextData()
	{
		Globals.PC = 0x00400000;
		Globals.nPC = 0x00400004;
	}
}
