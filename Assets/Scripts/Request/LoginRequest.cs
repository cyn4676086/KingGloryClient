using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;
using UnityEngine.SceneManagement;

public class LoginRequest : Request{
    [HideInInspector]
    public string UserName;
    [HideInInspector]
    public string Password;
    public override void DefaultRequest()
    {
        //构造参数
        var data = new Dictionary<byte, object>();

        data.Add((byte)ParaCode.UserName,UserName);
        data.Add((byte)ParaCode.Password,Password);

        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
       
    }

    public override void OnEvent(EventData data)
    {
        
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            print("登录成功");
            Destroy(GameObject.Find("CanvasLogin"));
            //跳转场景
            UIManager.GetInstance().PushPanel(UIPanelType.IndexPanel);

        }
        else
        {
            print("登录失败");
        }
    }


}
