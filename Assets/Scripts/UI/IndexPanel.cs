using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexPanel : BasePanel
{

	
	public void OnClick()
	{
		UIManager.GetInstance().PopPanel();
		UIManager.GetInstance().PushPanel(UIPanelType.LodingPanel);
	}
	
}

