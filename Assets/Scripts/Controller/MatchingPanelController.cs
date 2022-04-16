using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPanelController : MonoBehaviour {
	private string HeroName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnBtn()
	{
		transform.Find("Match").gameObject.SetActive(false);
		MatchingRequest.Instance.MatchingStartRequest(HeroName);
	}

	public void HeroHanBing()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "寒冰射手";
		HeroName = "HanBing";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
	public void HeroLeiDian()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "雷电之王";
		HeroName = "LeiDian";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
	public void HeroJianSheng()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "无影剑圣";
		HeroName = "JianSheng";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
}
