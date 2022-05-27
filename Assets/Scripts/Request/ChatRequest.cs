using Common;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatRequest : Request {
    public InputField input;
    [HideInInspector]
    public string Hello;

    

    public override void DefaultRequest()
    {
        //构造参数
        var data = new Dictionary<byte, object>();

        data.Add((byte)ParaCode.Chat,Hello);
        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            //print("发送成功");
        }
        else
        {
            //print("发送失败");
        }
    }

    public void OnChatBtn()
    {
        Hello = input.text;
        DefaultRequest();
    }

    public override void OnEvent(EventData data)
    {
        
    }
}
