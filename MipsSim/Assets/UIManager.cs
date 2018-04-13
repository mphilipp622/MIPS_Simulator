using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using MIPS_Simulator;
using System;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	private string _path;

	// lookup tables for decoding into strings
	private Dictionary<Tuple<byte, byte>, string> rFormat;
	private Dictionary<byte, string> iFormat;
	private Dictionary<byte, string> jFormat;

	[SerializeField]
	Text console;

	private ScaleText consoleText, programText;

	[SerializeField]
	GameObject inputPanelInt, inputPanelString;

	// Alias for cleaner lookup of register hash table
	private Dictionary<byte, Register> registers
	{
		get
		{
			return OperationManager.registers.registerTable;
		}
	}

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)
			Destroy(gameObject);
	}

	void Start ()
	{

		// OpCode Hash Tables that use strings. Will further be used in OperationManager and UIManager
		rFormat = new Dictionary<Tuple<byte, byte>, string>()
		{
			{ new Tuple<byte, byte>(0, 0), "SLL"		},
			{ new Tuple<byte, byte>(0, 2), "SRL"		},
			{ new Tuple<byte, byte>(0, 3), "SRA"		},
			{ new Tuple<byte, byte>(0, 4), "SLLV"		},
			{ new Tuple<byte, byte>(0, 6), "SRLV"		},
			{ new Tuple<byte, byte>(0, 7), "SRAV"		},
			{ new Tuple<byte, byte>(0, 8), "JR"			},
			{ new Tuple<byte, byte>(0, 9), "JALR"		},
			{ new Tuple<byte, byte>(0, 10), "MOVZ"      },
			{ new Tuple<byte, byte>(0, 11), "MOVN"      },
			{ new Tuple<byte, byte>(0, 12), "SYSCALL"   },
			{ new Tuple<byte, byte>(0, 16), "MFHI"		},
			{ new Tuple<byte, byte>(0, 17), "MTHI"		},
			{ new Tuple<byte, byte>(0, 18), "MFLO"		},
			{ new Tuple<byte, byte>(0, 19), "MTLO"		},
			{ new Tuple<byte, byte>(0, 24), "MULT"		},
			{ new Tuple<byte, byte>(0, 25), "MULTU"		},
			{ new Tuple<byte, byte>(0, 26), "DIV"		},
			{ new Tuple<byte, byte>(0, 27), "DIVU"		},
			{ new Tuple<byte, byte>(0, 32), "ADD"		}, 
			{ new Tuple<byte, byte>(0, 33), "ADDU"		},
			{ new Tuple<byte, byte>(0, 34), "SUB"		},
			{ new Tuple<byte, byte>(0, 35), "SUBU"		},
			{ new Tuple<byte, byte>(0, 36), "AND"		},
			{ new Tuple<byte, byte>(0, 37), "OR"		},
			{ new Tuple<byte, byte>(0, 38), "XOR"		},
			{ new Tuple<byte, byte>(0, 39), "NOR"		},
			{ new Tuple<byte, byte>(0, 42), "SLT"		},
			{ new Tuple<byte, byte>(0, 43), "SLTU"		}
		};

		iFormat = new Dictionary<byte, string>()
		{
			{ 4, "BEQ"		},
			{ 5, "BNE"		},
			{ 6, "BLEZ"		},
			{ 7, "BGTZ"		},
			{ 8, "ADDI"		},
			{ 9, "ADDIU"	},
			{ 10, "SLTI"	},
			{ 11, "SLTIU"	},
			{ 12, "ANDI"	},
			{ 13, "ORI"		},
			{ 14, "XORI"	},
			{ 15, "LUI"		},
			{ 32, "LB"		},
			{ 33, "LH"		},
			{ 35, "LW"		},
			{ 36, "LBU"		},
			{ 37, "LHU"		},
			{ 40, "SB"		},
			{ 41, "SH"		},
			{ 43, "SW"		}
		};

		jFormat = new Dictionary<byte, string>()
		{
			{ 2, "J"	},
			{ 3, "JAL"	}
		};

		consoleText = GameObject.FindGameObjectWithTag("ConsoleContent").GetComponent<ScaleText>();
		programText = GameObject.FindGameObjectWithTag("TextContent").GetComponent<ScaleText>();
		//Byte[] temp = BitConverter.GetBytes(0xFF001010);
		////Array.Reverse(temp);
		//for (uint i = 0, j = 3; i < 4; i++)
		//{
		//	Globals.staticData.Add(0x10010000 + i, temp[j - i]);
		//	//Debug.Log(String.Format("{0:X}", Globals.staticData[0x10010000 + i]));
		//}
		//Byte[] tempBytes = new Byte[2];
		//for (uint i = 0; i < 2; i++)
		//	tempBytes[i] = Globals.staticData[0x10010000 + i];

		//Array.Reverse(tempBytes);
		//short tempVal = BitConverter.ToInt16(tempBytes, 0);
		//Debug.Log(tempVal);
		////Array.Reverse(temp);
		////Array.Reverse(temp);
		////short tempVal = BitConverter.ToInt16(temp, 2);
		//Debug.Log(String.Format("{0:X}", tempVal));
	}
	
	void Update ()
	{
		
	}

	public void WriteDecodedRFormat(byte op, byte rs, byte rt, byte rd, byte shamt, byte funct)
	{
		Tuple<byte, byte> newOp = new Tuple<byte, byte>(op, funct);

		string newLine = null;

		if (funct >= 32 || funct == 10 || funct == 11) // format op, rd, rs, rt
			newLine = String.Format("{0}    {1}, {2}, {3}\n", rFormat[newOp], registers[rd].alias, registers[rs].alias, registers[rt].alias);
		else if (funct >= 0 && funct <= 3) // Format op, rd, rt, shamt
			newLine = String.Format("{0}    {1}, {2}, {3}\n", rFormat[newOp], registers[rd].alias, registers[rt].alias, Convert.ToString(shamt));
		else if (funct >= 4 && funct <= 7) // Format op, rd, rt, rs
			newLine = String.Format("{0}    {1}, {2}, {3}\n", rFormat[newOp], registers[rd].alias, registers[rt].alias, registers[rs].alias);
		else if (funct == 8 || funct == 9) // JR and JALR
			newLine = String.Format("{0}    {1}\n", rFormat[newOp], registers[rs].alias);
		else if (funct == 12)
			newLine = "SYSCALL\n";
		else if (funct == 16 || funct == 18) //MFHI, MFLO
			newLine = String.Format("{0}    {1}\n", rFormat[newOp], registers[rd].alias);
		else if (funct == 17 || funct == 19) // MTHI, MTLO
			newLine = String.Format("{0}    {1}\n", rFormat[newOp], registers[rs].alias);
		else if (funct >= 24 && funct <= 27) // MULT, DIV, etc.
			newLine = String.Format("{0}    {1}, {2}\n", rFormat[newOp], registers[rs].alias, registers[rt].alias);

		programText.SetText(newLine);
	}

	public void WriteDecodedIFormat(byte op, byte rs, byte rt, dynamic immediate)
	{
		string newLine = null;

		if (op == 4 || op == 5) // BEQ, BNE
			newLine = String.Format("{0}    {1}, {2}, {3}\n", iFormat[op], registers[rs].alias, registers[rt].alias, Convert.ToString(immediate * 4));
		else if (op == 6 || op == 7) // BLEZ, bgtz
			newLine = String.Format("{0}    {1}, {2}\n", iFormat[op], registers[rs].alias, Convert.ToString(immediate * 4));
		else if (op >= 8 && op <= 14) // addi, andi, etc
			newLine = String.Format("{0}    {1}, {2}, {3}\n", iFormat[op], registers[rt].alias, registers[rs].alias, Convert.ToString(immediate));
		else if (op == 15) // lui
			newLine = String.Format("{0}    {1}, {2}\n", iFormat[op], registers[rt].alias, Convert.ToString(immediate));
		else if (op >= 32) // loads and stores
			newLine = String.Format("{0}    {1}, {2}({3})\n", iFormat[op], registers[rt].alias, Convert.ToString(immediate), registers[rs].alias);

		programText.SetText(newLine);
	}

	public void WriteDecodedJFormat(byte op, uint address)
	{
		string newLine = string.Format("{0}    {1}\n", jFormat[op], Convert.ToString(address));

		programText.SetText(newLine);
	}

	/* 
	Turns passed in GameObject on and off depending on current active state.
	Use with UI.Button objects.
	*/
	public void ToggleMenu(GameObject obj)
	{
		obj.SetActive(!obj.activeSelf); // toggle the game object on/off
	}
	
	public void OpenFile()
	{
		WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "s", false));
	}
	
	void WriteResult(string[] paths)
	{
        if (paths.Length == 0) {
            return;
        }

        _path = "";
        foreach (var p in paths) {
            _path += p + "\n";
        }

		Globals.parser = new Parser(_path);
		//Debug.Log(Globals.parser.GetLine());
	}

    void WriteResult(string path)
	{
        _path = path;

		Globals.filePath = _path;
		Globals.parser = new Parser(_path);
	}

	public void PrintInt(dynamic val)
	{
		consoleText.SetText(val);
	}

	public void PrintString(string newString)
	{
		consoleText.SetText(newString);
	}

	public void ReadInt()
	{
		inputPanelInt.SetActive(true);
		inputPanelInt.GetComponentInChildren<InputField>().contentType = InputField.ContentType.IntegerNumber;
		inputPanelInt.GetComponentInChildren<InputField>().ActivateInputField();
		//return newVal;
	}

	public void SetInt()
	{
		OperationManager.registers.registerTable[2].value = Convert.ToInt32(inputPanelInt.GetComponentInChildren<InputField>().text);
		//Globals.intToDisplay = Convert.ToInt32(inputPanel.GetComponentInChildren<InputField>().text);
	}

	public void ReadString()
	{
		inputPanelString.SetActive(true);
		inputPanelString.GetComponentInChildren<InputField>().contentType = InputField.ContentType.Alphanumeric;
		inputPanelString.GetComponentInChildren<InputField>().ActivateInputField();
	}

	public void SetString()
	{
		
	}

	// Called on by NextLineButton in the scene
	public void ReadNextLine()
	{
		Globals.parser.ExecuteLine();
	}
}
