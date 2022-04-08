using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginOnClick : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void OnLoginBtnClick()
	{
		UIManager.GetInstance().PopPanel();
		UIManager.GetInstance().PushPanel(UIPanelType.LodingPanel);

	}
}
