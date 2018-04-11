using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using MIPS_Simulator;

public class UIManager : MonoBehaviour {
	
	private string _path;
	
	void Start ()
	{
		
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
}
