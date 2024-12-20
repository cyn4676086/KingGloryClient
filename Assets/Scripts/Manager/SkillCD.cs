﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCD : MonoBehaviour
{
	public Image cdImg;
	public ETCButton btn;
	public float Skill_time;
	// Use this for initialization
	void Start()
	{
		cdImg = transform.Find("cdImg").GetComponent<Image>();
		btn = GetComponent<ETCButton>();
	}

	// Update is called once per frame
	void Update()
	{

	}
	public void OnBtn()
	{
		cdImg.fillAmount = 1;
		cor=StartCoroutine(StartCD());
	}

	private IEnumerator StartCD()
	{
		//计算cd后的时间 禁用按钮
		var t = Time.time + Skill_time;
		btn.activated = false;
		while (true)
		{
			cdImg.fillAmount = (t - Time.time) / Skill_time;
			if (t <= Time.time)
			{
				btn.activated = true;
				break;
			}
			yield return new WaitForEndOfFrame();
		}

	}
	private Coroutine cor;
	public void StopCD()
    {
		StopCoroutine(cor);
		btn.activated = true;
		cdImg.fillAmount = 0;
	}
}
