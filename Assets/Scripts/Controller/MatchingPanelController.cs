using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public void OnReturnBtn()
	{
		SceneManager.LoadScene("GameHall");
	}

	public void HeroHanBing()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "寒冰射手";
		transform.Find("HeroInfo").GetComponent<Text>().text = "普通攻击：向范围内目标发射箭，造成伤害。"+ "\r\n"+ "一技能：向前发射光束,碰撞到敌人时造成巨额伤害。" + "\r\n" + "二技能：在技能生效攻速会增加一倍，增加伤害。" + "\r\n" + "三技能：向前发射冰冻束，将冰冻敌人一段时间。";
		HeroName = "HanBing";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
	public void HeroLeiDian()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "雷电之王";
		transform.Find("HeroInfo").GetComponent<Text>().text = "普通攻击：向目标发射电弧，造成伤害。" + "\r\n" + "一技能：经过蓄力向前发射雷电，碰撞到敌人时造成巨额伤害。" + "\r\n" + "二技能：生成一个雷电场，在雷电场内的敌人都会被减速90%。" + "\r\n" + "三技能：向前跳跃一段位移，在落地时造成巨额伤害，并将敌人击飞。";
		HeroName = "LeiDian";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
	public void HeroJianSheng()
	{
		transform.Find("HeroName").GetComponent<Text>().text = "无影剑圣";
		transform.Find("HeroInfo").GetComponent<Text>().text = "普通攻击：挥动武器造成伤害。" + "\r\n" + "一技能:：使用技能后会切换到跑步动作。移动速度获得大幅度增加，攻击速度增加。" + "\r\n" + "二技能：在二技能持续时间内进入冥想状态，持续回血，该状态可以被打断。" + "\r\n" + "三技能：自动搜索范围内血量最低的敌人，突进到敌人的位置进行收割。若以该技能击杀敌人，那么三技能可以瞬间刷新。";
		HeroName = "JianSheng";
		transform.Find("BeforeMatch").gameObject.SetActive(false);
	}
}
