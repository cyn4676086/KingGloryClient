using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill1_CD : MonoBehaviour {
    private Image cdImg;
	private ETCButton btn;
	public float Skill1_time = 2.5f;
    // Use this for initialization
    void Start () {
		cdImg = transform.Find("cdImg").GetComponent<Image>();
		btn = GetComponent<ETCButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnBtn()
    {
		cdImg.fillAmount = 1;
		StartCoroutine(StartCD());
    }

    private IEnumerator StartCD()
    {
		//计算cd后的时间 禁用按钮
		var t = Time.time + Skill1_time;
		btn.activated = false;
        while (true)
        {
			cdImg.fillAmount = (t - Time.time) / Skill1_time;
            if (t <= Time.time)
            {
				btn.activated = true;
				break;
            }
			yield return new WaitForEndOfFrame();
        }
		
    }
}
