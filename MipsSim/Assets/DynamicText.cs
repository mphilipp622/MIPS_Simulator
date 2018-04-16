using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour {

	[SerializeField]
	Image highlighter;

	[SerializeField]
	Text text;

	private void Awake()
	{
		//text = GetComponent<Text>();
		//highlighter = transform.GetChild(0).GetComponent<Image>();
		highlighter.gameObject.SetActive(false); // turn off highlighter initially
	}

	void Start () {
		
		
	}
	
	void Update () {
		
	}

	public void SetText(string newLine)
	{
		//string newLine = string.Format("{0:X} \t{1:X}\n", addr, data);

		text.text = newLine;
	}

	// switches highlight on or off depending on current highlight state
	public void Highlight()
	{
		highlighter.gameObject.SetActive(!highlighter.gameObject.activeSelf);
	}
}
