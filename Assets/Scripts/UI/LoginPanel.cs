using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
	public GameObject SigninPanel;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void OnRegister()
	{
		SigninPanel.SetActive(true);
		gameObject.SetActive(false);
	}
	public void OnLoginButton()
	{
		var Request = GetComponent<LoginRequest>();
		Request.UserName = transform.Find("NameField").GetComponent<InputField>().text;
		Request.Password = transform.Find("PWField").GetComponent<InputField>().text;
		Request.DefaultRequest();
	}
}