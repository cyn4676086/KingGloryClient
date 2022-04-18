using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontPanel : MonoBehaviour {
    public static DontPanel Instance;
    public  void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
