using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeiDianAtt : MonoBehaviour
{

    void Start()
    {
        SetCD();
        BuffManager();
        AddSkillFxListener();
    }
    void FixedUpdate()
    {
        if (isSkill3)
        {
            //位移
            transform.position = Vector3.Lerp(transform.position, Skill3Target.transform.position, 1f * Time.deltaTime);

        }
    }
    #region 雷电-普通攻击
    private bool Skilling;
    private float LDAttTime;
    public float LDAttCD;
    public int LDAttHurt;
    private int LDAttBuffHurt = 0;
    [HideInInspector]
    public int attIndex;
    public GameObject Thunder;
    public GameObject HandPosition;
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
        if (Skilling)
        {
            return false;
        }
        
            foreach (var item in list)
            {
                if (item.isDead == false && item.Group != Model.Group && Model.isDead == false)
                {
                    var dis = Vector3.Distance(item.transform.position, this.transform.position);
                    if (dis < 3f)
                    {
                        if (Time.time > LDAttTime)
                        {
                            LDAttTime = Time.time + LDAttCD;
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
        //实例化
        var obj = Instantiate(Thunder, HandPosition.transform.position, transform.rotation);
        var thunder = obj.GetComponent<Thunder>();
        thunder.Owner = Model.id;
        thunder.ThunderHurt = LDAttHurt + LDAttBuffHurt;
        if (Mathf.Round(attIndex / 1000) == 2)
        {
            thunder.target = BattleFieldManager.Instance.GetTowerByID(attIndex);
        }
        else if (Mathf.Round(attIndex / 1000) == 0)
        {
            thunder.target = BattleFieldManager.Instance.GetPlayerByID(attIndex).Model;
        }
        else if (Mathf.Round(attIndex / 1000) == 6)
        {
            thunder.target = BattleFieldManager.Instance.GetSoliderByID(attIndex);
        }
        GetComponent<PlayerMove>().isAttacking = false;
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
        else if (Mathf.Round(attIndex / 1000) == 0)
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
        GetComponent<PlayerMove>().isAttacking = true;
        attIndex = target;
    }
    #endregion
    #region 雷电射手-一技能
    public GameObject Skill1Effect;
    public float Skill1CD;
    public int Skill1Hurt = -100;
    private int Skill1BuffHurt;

    //技能命中检测
    private void AddSkillFxListener()
    {
        //添加技能侦听
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        //添加事件侦听
        if (tm != null) tm.CollisionEnter += Tm_CollisionEnter;
    }
    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
    {
        //print("技能命中");
        //技能命中
        if (e.Hit.transform.tag == "Player")
        {

            //发送请求，技能伤害
            if (e.Hit.transform.GetComponent<BodyModel>().id != BattleFieldManager.Instance.MyPlayerIndex)
            {
                //print("发送伤害");
                BattleFieldRequest.Instance.HurtRequest(e.Hit.transform.GetComponent<PlayerModel>().id, Skill1Hurt, Model.id);
            }
        }

    }


    internal void OnSkill1()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            if (item != Model)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 5f)
                {
                    attIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Skill1);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                    Skilling = true;
                }
            }
        }
    }
    public void OnSkill1Animation()
    {
        if (Skill1Effect == null) return;
        GetComponent<PlayerMove>().isAttacking = false;
        Skilling = false;
    }
    internal void PlaySkill(int target)
    {
        GetComponent<PlayerMove>().isAttacking = true;
        //print("动作" + GetComponent<PlayerMove>().isAttacking);
        //先转向到目标角色
        GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetPlayerByID(target).GetComponent<Transform>().position);
        var item = BattleFieldManager.Instance.GetPlayerByID(target);
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("skill1");
        Skill1Effect.SetActive(true);
        //print("技能动作触发");
        attIndex = target;
        if (BattleFieldManager.Instance.GetPlayerByID(target).Model.Group != this.Model.Group)
        {
            Skill1Effect.GetComponent<Transform>().LookAt(item.transform.position + Vector3.down);
        }
    }
    #endregion
    #region 雷电二技能
    public float Skill2DuringTime;
    public float Skill2CD;
    internal void OnSkill2()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            if (item != Model)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 7f)
                {
                    attIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Skill2);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                }
            }
        }
    }

    private Coroutine Skill2EffectTime;
    private IEnumerator SKill2During(float f, float Skill2DuringTime)
    {
        yield return new WaitForSeconds(Skill2DuringTime);
        LDAttCD = f;
    }
    internal void PlaySkill2()
    {
        float f = LDAttCD;
        LDAttCD = LDAttCD / 2;
        GetComponent<Animator>().SetTrigger("skill2");
        Instantiate(Resources.Load("Heros/LDSkill2") as GameObject, transform.position + 2 * Vector3.up, Quaternion.identity);
        StartCoroutine(SKill2During(f, Skill2DuringTime));
    }
    private void OnDisable()
    {
        //关闭协程
        StopCoroutine(Skill2EffectTime);
    }

    #endregion
    #region 雷电-三技能
    public float Skill3CD;
    public GameObject Skill3Effect;
    public PlayerMove Skill3Target;
    private bool isSkill3 = false;
    private int Skill3Hurt = -300;
    

    internal void OnSkill3()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            if (item != this.Model)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);

                if (dis < 7f)
                {
                    
                    attIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Skill3);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                    Skilling = true;
                }
            }
        }
    }
    public void OnSkill3Animation()
    {
        if (Skill3Effect == null) return;
        isSkill3 = false;
        Skill3Effect.SetActive(true);

    }
    public void OnSkill3AnimationDown()
    {
        if (this.Model == Skill3Target.Model)
        {
            return;
        }
        var distance = Vector3.Distance(transform.position, Skill3Target.transform.position);
        if (distance < 1.5)
        {
            if (Skill3Target.Model.id != BattleFieldManager.Instance.MyPlayerIndex)
            {
                BattleFieldRequest.Instance.HurtRequest(Skill3Target.Model.id, Skill3Hurt, Model.id);
            }
            Skill3Target.Model.FlyEnter();
        }

    }

    public void OnSkill3AnimationDone()
    {
        GetComponent<PlayerMove>().isAttacking = false;
        Skilling = false;
    }
    internal void PlaySkill3(int target)
    {
        Skill3Target = BattleFieldManager.Instance.GetPlayerByID(target);
        //先转向到目标角色
        GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetPlayerByID(target).GetComponent<Transform>().position);
        var item = BattleFieldManager.Instance.GetPlayerByID(target);
        //切换动画状态机
        isSkill3 = true;
        GetComponent<Animator>().SetTrigger("skill3");
        GetComponent<PlayerMove>().isAttacking = true;
        attIndex = target;
        if (BattleFieldManager.Instance.GetPlayerByID(target).Model.Group != this.Model.Group)
        {
            Skill3Effect.GetComponent<Transform>().LookAt(item.transform.position);
        }
    }
    #endregion

    #region 人物属性动态增长
    private void BuffManager()
    {
        StartCoroutine(LDAttBuff());
        StartCoroutine(LDExpAttBuff());
    }

    private IEnumerator LDAttBuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            //生命值加成
            GetComponent<HealthBar>().defaultHealth += 970;
            BattleFieldRequest.Instance.HurtRequest(Model.id, 970, BattleFieldManager.Instance.MyPlayerIndex);
            //普通攻击加成
            LDAttCD *= 0.9f;
            LDAttHurt = (int)(LDAttHurt * 1.1f);
            //一技能加成
            Skill1CD *= 0.7f;
            Skill1Hurt = (int)(Skill1Hurt * 1.3f);
            //二技能加成
            Skill2CD *= 0.9f;
            Skill2DuringTime *= 1.1f;
            //三技能加成
            Skill3CD *= 0.8f;
            Skill3Hurt = (int)(Skill3Hurt * 1.1f);
            SetCD();
        }
    }
    private IEnumerator LDExpAttBuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            int buff = GetComponent<PlayerModel>().Buff;

            if (buff > 0)
            {
                for (; buff >= 0; buff--)
                {
                    //生命值加成
                    GetComponent<HealthBar>().defaultHealth += 60;
                    BattleFieldRequest.Instance.HurtRequest(Model.id, 60, BattleFieldManager.Instance.MyPlayerIndex);
                    //普通攻击加成
                    LDAttBuffHurt -= 10;
                    //一技能加成
                    Skill1BuffHurt -= 20;
                }
                GetComponent<PlayerModel>().Buff = 0;
            }
        }
    }
    private void SetCD()
    {
        GameObject.FindGameObjectWithTag("skill1").GetComponent<SkillCD>().Skill_time = Skill1CD;
        GameObject.FindGameObjectWithTag("skill2").GetComponent<SkillCD>().Skill_time = Skill2CD;
        GameObject.FindGameObjectWithTag("skill3").GetComponent<SkillCD>().Skill_time = Skill3CD;
    }
    #endregion
}
