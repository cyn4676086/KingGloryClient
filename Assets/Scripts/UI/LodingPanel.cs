using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingPanel : BasePanel {

	// 协程延迟加载进度条
	void Start () {
		StartCoroutine(Loding());
	}

    private IEnumerator Loding()
    {
        var t = Time.time;
        var img = transform.Find("bottom/Image").GetComponent<Image>();
        img.fillAmount = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            img.fillAmount = (Time.time - t) / 1;
            if (img.fillAmount==1)
            {
                UIManager.GetInstance().PopPanel();
                UIManager.GetInstance().PushPanel(UIPanelType.MainPanel);
                //SceneManager.LoadScene("GameHall");
                break;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    
}
