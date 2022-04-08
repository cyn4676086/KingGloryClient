


using System;
using UnityEngine;
using System.Collections.Generic;

public class UIManager
{
    private static UIManager Instance;
    private Transform CanvasTransform;
    UIPanelTypeJson json;

    //打开的顺序  堆 
    Stack<BasePanel> PanelStack = new Stack<BasePanel>();
    private Dictionary<UIPanelType, GameObject> PanelCach = new Dictionary<UIPanelType, GameObject>();



    public static UIManager GetInstance()
    {
        //惰性实例化
        if (Instance == null)
        {
            Instance = new UIManager();
        }
        return Instance;
    }
    /// <summary>
    /// 显示一个面板
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType)
    {
        //调用被挡住的OnPause
        if (PanelStack.Count != 0)
        {
            PanelStack.Peek().OnPause();

        }

        BasePanel panel = GetPanel(panelType);
        PanelStack.Push(panel);

        panel.OnEnter();
    }

    public void PopPanel()
    {
        if (PanelStack.Count == 0)
            return;
        BasePanel panel = PanelStack.Pop();
        panel.OnExit();

        if (PanelStack.Count != 0)
            PanelStack.Peek().OnResume();
    }




    private UIManager()
    {
        ParseUIPanelTypeJson();
        CanvasTransform = GameObject.Find("Canvas").transform;
    }


    private void ParseUIPanelTypeJson()
    {
        //加载json文件
        TextAsset t = Resources.Load<TextAsset>("UIPanelType");
        json = JsonUtility.FromJson<UIPanelTypeJson>(t.text);
    }
    /// <summary>
    /// 创建一个面板并且显示
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        GameObject instPanel = PanelCach.TryGetValueByNN(panelType);
        //判断缓存里面有没有，如果没有，创建新的，如果有拿缓存里的
        if (instPanel == null)
        {

            //通过名字找路径
            string path = "";
            foreach (var item in json.PanelList)
            {
                
                if (item.PanelName == panelType.ToString())
                {
                    path = item.PanelPath;
                   
                }
            }
           
            instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform, false);
            PanelCach.Add(panelType, instPanel);
        }
        

        return instPanel.GetComponent<BasePanel>();
    }
}

//序列化
[Serializable]
public class UIPanelTypeJson
{
    public UIPanelInfo[] PanelList;
}
//序列化
[Serializable]
public class UIPanelInfo
{
    public string PanelName;
    public string PanelPath;

}
