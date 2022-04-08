using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingPanelController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnBtn()
	{
		transform.Find("Match").gameObject.SetActive(false);
		MatchingRequest.Instance.MatchingStartRequest();
	}
}
