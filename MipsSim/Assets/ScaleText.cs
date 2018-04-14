using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleText : MonoBehaviour
{
	RectTransform textArea, contentArea;
	Text text;

	[SerializeField]
	float minHeight = 53.3f;

	void Start ()
	{
		contentArea = GetComponent<RectTransform>();
		textArea = transform.Find("Text").GetComponent<RectTransform>();
		text = transform.Find("Text").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Scale();
	}

	// Will be called by parser when printing out text. Every time a new line is read, we scale
	void Scale()
	{
		contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, contentArea.sizeDelta.y + minHeight);
		textArea.sizeDelta = new Vector2(textArea.sizeDelta.x, textArea.sizeDelta.y + minHeight);
	}

	// Used for syscall Print_Int
	public void SetText(int val)
	{
		text.text += val;
	}

	// Used for printing lines of data. Will generally be used on Program data window
	public void SetText(string newLine)
	{
		text.text += newLine;

		if(newLine.EndsWith("\n")) // if line ends, we need to scale text area downwards
			Scale();
	}
}
