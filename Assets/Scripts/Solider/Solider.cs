using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Solider : BodyModel {

	public List<GameObject>FinalTargets;
    public List< GameObject>CurTargets;
    public int SoliderAtt=-10;

    void Start () {
        BattleFieldManager.Instance.SoliderList.Add(this);
        HP = 200;
        SetHealth();
	}
	public void Init(List<GameObject> target,int Group,int id)
    {
		this.FinalTargets= target;
        this.Group = Group;
        this.id = id;
    }
	internal GameObject FinalTarget()
    {
        //BodyModel数据驱动 判断死亡
        if (FinalTargets[0].GetComponent<BodyModel>().isDead==true)
        {
            FinalTargets.Remove(FinalTargets[0]);
        }
		return FinalTargets[0];
    }

    internal GameObject GetCurTarget()
    {
        if (CurTargets.Count == 0)
            return null;
        if (CurTargets[0]==null|| CurTargets[0].GetComponent<BodyModel>().isDead == true)
        {
            CurTargets.Remove(CurTargets[0]);
        }
        if (CurTargets.Count == 0)
            return null;
        return CurTargets[0];
    }

    void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        var body = other.GetComponent<BodyModel>();
        //判断阵营 判断死亡
        if (body == null)
        {
            return;
        }
        if (body.isDead == false && body.Group != Group)
        {
            //print("添加当前目标：" + body.Group + "：" + Group + "：" + body.gameObject.name);
            CurTargets.Add(body.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var body = other.GetComponent<BodyModel>();
        if (body == null)
        {
            return;
        }
        if (body.isDead == false && body.Group != Group)
        {
            //print("退出cur目标：" + body.Group + ":" + Group + ":" + body.gameObject.name);
        }
    }
    private void OnDestory()
    {
        BattleFieldManager.Instance.SoliderList.Remove(this);
    }

    internal void SoliderDeathAnimator()
    {
        isDead = true;
        //小兵立即停止移动 
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<Animator>().SetTrigger("death");
        Debug.LogError("小兵死亡动作");
        //动作播放完毕Event销毁
       
    }
    public void OnSoliderDeath()
    {
        
        Destroy(gameObject);
    }
    public void OnSoliderAtt()
    {
        print(GetCurTarget().GetComponent<BodyModel>().id + "    " + id);
        if (BattleFieldManager.Instance.Player.Model.Group == Group)
        {
            BattleFieldRequest.Instance.HurtRequest (GetCurTarget().GetComponent<BodyModel>().id, SoliderAtt,id);
        }
    }
    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(id,hurtValue, ObjectID);
    }
    
}
