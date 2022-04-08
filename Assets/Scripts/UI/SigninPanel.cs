using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigninPanel : MonoBehaviour
{
	public GameObject LoginPanel;
	void Start()
	{

	}
	void Update()
	{

	}
	public void OnLogin()
	{
		LoginPanel.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnSigninButton()
	{
		var Request = GetComponent<SigninRequest>();
		Request.UserName = transform.Find("NameField").GetComponent<InputField>().text;
		Request.Password = transform.Find("PWField").GetComponent<InputField>().text;
		Request.DefaultRequest();
	}
}