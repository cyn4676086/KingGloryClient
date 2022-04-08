using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour {
    public TextAsset json;


	void Start () {
        Debug.LogError( JsonUtility.FromJson<InputData>(json.text).pepole[0].fname);
	}
	
	void Update () {
		
	}
}


[Serializable]
public class InputData
{
    public InputDataEntry[] pepole;
}

[Serializable]
public class InputDataEntry
{
    public string fname;
    public string lname;
}



