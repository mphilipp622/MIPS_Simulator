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
		textArea = transform.GetChild(0).GetComponent<RectTransform>();
		text = transform.GetChild(0).GetComponent<Text>();
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

	public void SetText(string newLine)
	{
		text.text += newLine + "\n";
		Scale();
	}
}
