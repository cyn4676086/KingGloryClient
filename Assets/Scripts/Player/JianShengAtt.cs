using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JianShengAtt : MonoBehaviour
{

    void Start()
    {
    }
    void FixedUpdate()
    {
        if (isSkill3)
        {
            //位移
            transform.position = Vector3.Lerp(transform.position, Skill3Target.transform.position, 6f*Time.deltaTime);
            var dis = Vector3.Distance(transform.position, Skill3Target.transform.position);
            if (dis < 2 && Skill3Ani == true)
            {
                GetComponent<Animator>().SetTrigger("skill3");
                print("三技能");
                Skill3Ani = false;
                isSkill3 = false;
            }
        }
    }


    #region 剑圣-普通攻击
    private float JSAttTime;
    public float JSAttCD;
    [HideInInspector]
    public int attIndex;
    public GameObject JianQi;
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
        foreach (var item in list)
        {
            if (item.isDead == false && item.Group != Model.Group && Model.isDead == false)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);
                if (dis < 2f)
                {
                    if (Time.time > JSAttTime)
                    {
                        JSAttTime = Time.time + JSAttCD;
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
        var obj = Instantiate(JianQi, HandPosition.transform.position, transform.rotation);
        var Jianqi = obj.GetComponent<Jianqi>();
        Jianqi.Owner = Model.id;

        if (Mathf.Round(attIndex / 1000) == 2)
        {
            Jianqi.target = BattleFieldManager.Instance.GetTowerByID(attIndex);
        }
        else if (Mathf.Round(attIndex / 1000) == 0)
        {
            Jianqi.target = BattleFieldManager.Instance.GetPlayerByID(attIndex).Model;
        }
        else if (Mathf.Round(attIndex / 1000) == 6)
        {
            Jianqi.target = BattleFieldManager.Instance.GetSoliderByID(attIndex);
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
    #region -一技能
    public float Skill1DuringTime;
    internal void OnSkill1()
    {
        BattleFieldRequest.Instance.AttackRequest(Model.id, Model.id, Common.AttackType.Skill1);
    }

    private Coroutine Skill1EffectTime;

    internal void PlaySkill1()
    {
        GetComponent<PlayerMove>().Speed = 1.8f;
        GetComponent<Animator>().SetBool("skill1", true);
        JSAttCD /= 2;
        print("加速");
        StartCoroutine(SKill1During( Skill2DuringTime));
    }

    private IEnumerator SKill1During( float Skill1DuringTime)
    {
        yield return new WaitForSeconds(Skill1DuringTime);
        GetComponent<Animator>().SetBool("skill1", false);
        GetComponent<PlayerMove>().Speed = 1.0f;
        JSAttCD *= 2;

        print("结束");
    }
    #endregion
    #region 剑圣二技能
    public float Skill2DuringTime;
    public int Skill2Cure;
    internal void OnSkill2()
    {
        BattleFieldRequest.Instance.AttackRequest(Model.id, Model.id, Common.AttackType.Skill2);
    }

    private Coroutine Skill2EffectTime;
   
    internal void PlaySkill2()
    {
        GetComponent<Animator>().SetTrigger("skill2");
        Instantiate(Resources.Load("Heros/JSSkill2") as GameObject, transform.position, Quaternion.identity);
        StartCoroutine(SKill2During(Skill2Cure, Skill2DuringTime));
    }

    private IEnumerator SKill2During(int Hurt, float Skill2DuringTime)
    {
        var t = Time.time;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Model.id == BattleFieldManager.Instance.MyPlayerIndex && Model.isDead == false)
            {
                BattleFieldRequest.Instance.HurtRequest(Model.id, Hurt, BattleFieldManager.Instance.MyPlayerIndex);
            }
            if (Time.time - t >= Skill2DuringTime || GetComponent<NavMeshAgent>().velocity.magnitude >= 0.2f){
                GetComponent<Animator>().SetTrigger("idle");
                break;
            }
           
        }

    }
    #endregion
    #region 剑圣-三技能
    [HideInInspector]
    public PlayerMove Skill3Target;
    private bool isSkill3 = false;
    private int Skill3Hurt = -300;
    private bool Skill3Ani=true;
    internal void OnSkill3()
    {
        //获取到攻击对象，如果有，发送攻击指令
        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            if (item != this.Model)
            {
                var dis = Vector3.Distance(item.transform.position, this.transform.position);

                if (dis < 700f)
                {
                    attIndex = item.GetComponent<PlayerModel>().id;
                    //发送攻击请求
                    BattleFieldRequest.Instance.AttackRequest(Model.id, attIndex, Common.AttackType.Skill3);
                    GetComponent<Transform>().LookAt(item.GetComponent<Transform>().position);
                }
            }
        }
    }
    public void OnSkill3Animation()
    {
        
        var distance = Vector3.Distance(transform.position, Skill3Target.transform.position);
        if (distance < 2)
        {
            if (Skill3Target.Model.id != BattleFieldManager.Instance.MyPlayerIndex)
            {
                BattleFieldRequest.Instance.HurtRequest(Skill3Target.Model.id, Skill3Hurt, Model.id);
            }
        }
        GetComponent<PlayerMove>().isAttacking = false;
        Skill3Ani = true;
    }
    internal void PlaySkill3(int target)
    {
        Skill3Target = BattleFieldManager.Instance.GetPlayerByID(target);
        //先转向到目标角色
        GetComponent<Transform>().LookAt(BattleFieldManager.Instance.GetPlayerByID(target).GetComponent<Transform>().position);
        var item = BattleFieldManager.Instance.GetPlayerByID(target);
        //切换动画状态机
        GetComponent<Animator>().SetTrigger("channel");
        isSkill3 = true;
        GetComponent<PlayerMove>().isAttacking = true;
        attIndex = target;

    }
    #endregion
}
