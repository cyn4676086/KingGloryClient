using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanBingAtt : MonoBehaviour
{
    void Start()
    {
        AddSkill1FxListener();
    }
    void LateUpdate()
    {
        SetSkill1Position();
    }
    #region 寒冰射手-普通攻击
    private float HBAttTime;
    public float HBAttCD=0.7f;
    [HideInInspector]
    public int attIndex;
    public GameObject Arrow;
    public GameObject HandPosition;
    [HideInInspector]
    public bool isAttcking = false;
    public PlayerModel Model { get { return GetComponent<PlayerModel>(); } }
   
    public void OnAttBtn()
    {
        if (GetTarget(BattleFieldManager.Instance.playerList))
        {
            return;
        }
        if (GetTarget(BattleFieldManager.Instance.TowerList))
        {
            return;
        }
        if (GetTarget(BattleFieldManager.Instance.SoliderList))
        {
            return;
        }

    }

    private bool GetTarget(List<BodyModel> list)
    {
        //获取到攻击对象英雄，如果有，发送攻击指令
        foreach (var item in list)
        {
            if (item.isDead==false&&item.Group!=Model.Group)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 7f)
                {
                    if (Time.time > HBAttTime)
                    {
                        HBAttTime = Time.time + HBAttCD;
                        attIndex = item.id;
                        //发送攻击请求
                        BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Normal);
                        GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void OnAttackAnimation()
    {
        //弓箭实例化
        var obj = Instantiate(Arrow, HandPosition.transform.position, transform.rotation);
        var arrow = obj.GetComponent<Arrow>();
        arrow.Owner = Model.id;

        if (Mathf.Round(attIndex / 1000) == 2)
        {
            arrow.target = BattleFieldManager.Instance.GetTowerByID(attIndex);
        }
        else if(Mathf.Round(attIndex / 1000) == 0)
        {
            arrow.target = BattleFieldManager.Instance.GetPlayerByID(attIndex).Model;
        }
        else if(Mathf.Round(attIndex / 1000) == 6)
        {
            arrow.target = BattleFieldManager.Instance.GetSoliderByID(attIndex);
        }
        isAttcking = false;
    }
    internal void PlayAtt(int target)
    {
        if (Mathf.Round(attIndex / 1000) == 2)
        {
            if (BattleFieldManager.Instance.GetTowerByID(target) == null)
                return;
            //先转向到目标角色
            GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetTowerByID(target).GetComponent<Transform>().position);
        }
        else if(Mathf.Round(attIndex / 1000) == 0)
        {
            if (BattleFieldManager.Instance.GetPlayerByID(target) == null)
                return;
            GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetPlayerByID(target).GetComponent<Transform>().position);
        }
        else if (Mathf.Round(attIndex / 1000) == 6)
        {
            if (BattleFieldManager.Instance.GetSoliderByID(target) == null)
                return;
            GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetSoliderByID(target).GetComponent<Transform>().position);
        }
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("attack");
        isAttcking = true;
        attIndex = target;
    }
    #endregion
    #region 寒冰射手-一技能
    public Transform Skill1Position;
    public GameObject Skill1Effect;

  

    private void AddSkill1FxListener()
    {
        //添加技能侦听
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        //添加事件侦听
        if (tm != null) tm.CollisionEnter += Tm_CollisionEnter;
    }
    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
    {
        //先判断是否命中Player
        if (e.Hit.transform.tag == "Player"||e.Hit.transform.tag=="Solider")
        {

            //发送请求，一技能伤害
            if (e.Hit.transform.GetComponent<BodyModel>().id != BattleFieldManager.Instance.MyPlayerIndex)
            {
                BattleFieldRequest.Instance.HurtRequest(e.Hit.transform.GetComponent<PlayerModel>().id,-80, Model.id);
            }
        }
        
    }
    private void SetSkill1Position()
    {
        //技能特效跟随英雄
        if (Skill1Effect != null && Skill1Position != null)
        {
            Skill1Effect.transform.position = Skill1Position.position;
        }
    }

    internal void OnSkill1()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            if (item != this)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 7f)
                {
                    attIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Skill1);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                }
            }
        }
    }
    public void OnSkill1Animation()
    {
        print("技能特效1");
        if (Skill1Effect == null) return;
        Skill1Effect.SetActive(true);
        print("技能特效2");
        isAttcking = false;
    }
    internal void PlaySkill(int target)
    {
        //先转向到目标角色
        GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetPlayerByID(target).GetComponent<Transform>().position);
        var item = BattleFieldManager.Instance.GetPlayerByID(target);
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("skill1");
        print("技能动作触发");
        isAttcking = true;
        attIndex = target;
        Skill1Effect.GetComponent<Transform>().LookAt(item.transform.position + Vector3.up);
    }
    #endregion
}
