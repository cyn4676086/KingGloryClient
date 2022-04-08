using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BodyModel
{
    [HideInInspector]
    public bool isMe = false;
    internal void ExpUp(int exp)
    {
        print("经验+1");
        transform.Find("LvFx").GetComponent<ParticleSystem>().Play();
    }
    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        print("发送普通攻击player扣血请求");
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }
}
