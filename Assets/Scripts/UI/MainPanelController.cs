using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanelController : BasePanel {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ShowPopUpPanel()
    {
		UIManager.GetInstance().PushPanel(UIPanelType.PopUp);
    }
	public void OnButton()
    {
		SceneManager.LoadScene("1v1");
    }
}
