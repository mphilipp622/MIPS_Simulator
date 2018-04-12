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

	[SerializeField]
	Text console;

	private ScaleText consoleText, programText;

	[SerializeField]
	GameObject inputPanelInt, inputPanelString;

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
}
