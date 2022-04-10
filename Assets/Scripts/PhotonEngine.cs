
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public static PhotonEngine Instance;


    public static PhotonPeer peer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //当前Gameobject不会随场景的销毁而销毁
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        //peer.Connect("127.0.0.1:5055", "MyGame");
        peer.Connect("114.55.64.119:5055", "MyGame");
    }

    void Update()
    {
        peer.Service();
    }

    private void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnEvent(EventData eventData)
    {
        DicTool.GetValue(RequestDic, (OperationCode)eventData.Code).OnEvent(eventData);

        return;
        if ((byte)OperationCode.Chat == eventData.Code)
        {
            var data = eventData.Parameters;
            object intValue;
            data.TryGetValue((byte)ParaCode.Chat, out intValue);
            Debug.Log("收到服务器:" + intValue + eventData.Code);
        }


        switch (eventData.Code)
        {
            //case 1:
            //    //解析数据
            //    var data = eventData.Parameters;
            //    object intValue, StringValue;
            //    data.TryGetValue(1, out intValue);              
            //    data.TryGetValue(2, out StringValue);
            //    Debug.Log("收到服务器的响应Event推送，OpCode：1" + intValue.ToString() + ":" + StringValue.ToString());
            //    break;

            default:
                break;
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        DicTool.GetValue(RequestDic, (OperationCode)operationResponse.OperationCode).
            OnOperationResponse(operationResponse);

        return;
        switch (operationResponse.OperationCode)
        {
            case 1:
                Debug.Log("收到服务器的响应，OpCode：1");

                //解析数据
                var data = operationResponse.Parameters;
                object intValue;
                data.TryGetValue(1, out intValue);
                object StringValue;
                data.TryGetValue(2, out StringValue);
                Debug.Log("收到客户端的请求，OpCode：1" + intValue.ToString() + ":" + StringValue.ToString());
                break;
            default:
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.LogError(statusCode);
    }

    private Dictionary<OperationCode, Request> RequestDic = new Dictionary<OperationCode, Request>();

    public void AddRequest(Request r)
    {
        if (RequestDic.ContainsKey(r.OpCode)){
            return;
        }
        RequestDic.Add(r.OpCode, r);
    }
    public void RemoveRequest(Request r)
    {
        RequestDic.Remove(r.OpCode);
    }
    public Request GetRequest(OperationCode code)
    {
        return DicTool.GetValue(RequestDic, code);
    }
}
