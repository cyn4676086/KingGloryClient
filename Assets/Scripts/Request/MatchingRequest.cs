using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System;

public class MatchingRequest : Request
{
    public static MatchingRequest Instance;
    public new void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public override void DefaultRequest()
    {}

    

    internal void MatchingStartRequest(string PickedHeroName)
    {
        //构造参数
        var data = new Dictionary<byte, object>();
        data.Add((byte)ParaCode.ParaType, ParaCode.Matching_Start);
        data.Add((byte)ParaCode.HeroType, PickedHeroName);
        //发送
        PhotonEngine.peer.OpCustom((byte)OpCode, data, true);
        print("发送匹配请求");
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        //解析数据
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            print("匹配中");
        }
        else
        {
            print("请求失败");
        }
    }
    public override void OnEvent(EventData data)
    {
        print("match return");
        //解析数据
        ParaCode type = (ParaCode)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.ParaType);
        if (type == ParaCode.Matching_confirm)
        {
            var para=(string)DicTool.GetValue<byte, object>(data.Parameters, (byte)ParaCode.Matching_confirm);
            var list = para.Split(',');
            int playerindex = int.Parse(list[0]);
            string hero = list[1];
            string otherHero = list[2];
            //初始化角色
            BattleFieldManager.Instance.InitBattleField(playerindex,hero);
            BattleFieldManager.Instance.AddOnePlayer(playerindex == 1 ? 2 : 1,otherHero);
            //隐藏界面 开始战斗
            GameObject.Find("BattleFiledManager").GetComponent<BattleFieldDataManager>().enabled = true;
            GameObject.FindGameObjectWithTag("MatchingPanel").SetActive(false);
        }
    }
}
