using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpPanelController : BasePanel {
    private Text text;
    public float StartTime;
    public float CurTime;
    private float ExitTime;

    // Use this for initialization
    private void Awake()
    {
		text = transform.Find("Image/Text").GetComponent<Text>();
    }
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
        CurTime = Time.time - StartTime + ExitTime;
        text.text = "Time: " + (CurTime);
        
	}
	public void Close()
    {
		UIManager.GetInstance().PopPanel();
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        StartTime = Time.time;
        canvas.alpha = 0;
        canvas.DOFade(1, 0.2f);
    }
    public override void OnExit()
    {
        base.OnExit();
        ExitTime = CurTime;
        canvas.alpha = 1;
        canvas.DOFade(0, 0.2f);
    }
}
