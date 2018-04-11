using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MIPS_Simulator;

public class RegisterTextManager : MonoBehaviour
{
	Dictionary<byte, Text> registerText;

	void Start ()
	{
		registerText = new Dictionary<byte, Text>();
		for (byte i = 0; i < transform.childCount; i++)
			registerText.Add(i, transform.GetChild(i).GetComponent<Text>());

	}
	
	void Update ()
	{
		
	}

	public void SetRegisterText(byte reg, string regName, dynamic val)
	{
		registerText[reg].text = "$" + regName+ " = " + val;
	}
}
