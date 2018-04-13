using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MIPS_Simulator;
using System;

public class RegisterTextManager : MonoBehaviour
{
	Dictionary<int, Text> registerText;

	void Start ()
	{
		registerText = new Dictionary<int, Text>();
		for (int i = 0; i < transform.childCount; i++)
			registerText.Add(i, transform.GetChild(i).GetComponent<Text>());
	}
	
	void Update ()
	{

	}

	public void SetRegisterText(byte reg, string regName, dynamic val)
	{
		//Debug.Log(reg + "    " + regName + "    " + val);
		registerText[reg].text = regName+ " = " + val;
	}
}
