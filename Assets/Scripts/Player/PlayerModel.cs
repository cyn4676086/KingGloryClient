using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerModel : BodyModel
{
    [HideInInspector]
    public int Buff;
    [HideInInspector]
    public bool isMe = false;
    [HideInInspector]
    public string HeroName;
    void FixedUpdate()
    {
        if (isFly)
        {
            Flying();
        }
        if (isDown)
        {
            Dowing();
        }
    }
    internal void BuffUp(int money)
    {
        Buff += money;
        //transform.Find("LvFx").GetComponent<ParticleSystem>().Play();
    }
    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        //print("发送普通攻击player扣血请求");
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }
    #region  人物复活逻辑
    public int WaitTime=5;
    //协程复活计时
    internal void PlayerDead()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        isDead = true;
        GetComponent<Animator>().SetTrigger("death");
        //死亡动画 禁止玩家操作模型
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(true);
        }
        if (WaitTime <= 20)
        {
            //复活时间累次增长，最长25s
            WaitTime += 5;
        }
        StartCoroutine(WaitRebirth());
    }
    private IEnumerator WaitRebirth()
    {
        yield return new WaitForSeconds(WaitTime);
        if (id == 1)
        {
            transform.position = GameObject.Find("RedHero").transform.position;
        }
        if (id == 2)
        {
            transform.position = GameObject.Find("BlueHero").transform.position;
        }
        isDead = false;
        GetComponent<NavMeshAgent>().enabled = true;
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(false);
        }
        GetComponent<Animator>().SetTrigger("idle");
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            BattleFieldRequest.Instance.HurtRequest(id, (int)GetComponent<HealthBar>().defaultHealth, id);
        }
    }
    #endregion
    #region 人物被冰冻
    private Coroutine Freeze;
    public void FreezeEnter(int freezeHurt)
    {
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(true);
        }
        
        Freeze = StartCoroutine(FreezeExit( freezeHurt ));
    }
    private IEnumerator FreezeExit(int freezeHurt)
    {
       
        var t = Time.time;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            //if (id != BattleFieldManager.Instance.MyPlayerIndex&&isDead==false)
            //{
            //    BattleFieldRequest.Instance.HurtRequest(id, freezeHurt,BattleFieldManager.Instance.MyPlayerIndex);
            //}
            if (Time.time - t >= 5f)
            {
                break;
            }
            
        }
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(false);
        }
    }
    #endregion
    #region 人物被减速
    private Coroutine SpeedDown;
    public void SpeedDownEnter(float speed,float time)
    {
            GetComponent<PlayerMove>().Speed = speed;
            Freeze = StartCoroutine(SpeedDownExit(time));
    }
    private IEnumerator SpeedDownExit(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<PlayerMove>().Speed = 1;
    }
    #endregion
    #region 人物被击飞
    [HideInInspector]
    public bool isFly=false;
    private bool isDown;
    private Coroutine Fly;
    public void Flying()
    {
        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(true);
        }
        transform.position=Vector3.Lerp(transform.position,transform.position+Vector3.up,Time.deltaTime*3);
        
    }
    public void Dowing()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position +  Vector3.down, Time.deltaTime*3);

    }
    public void FlyEnter()
    {
        isFly = true;
        GetComponent<NavMeshAgent>().enabled = false;
        print("击飞开始");
        StartCoroutine(FlyExit());
    }
    
    private IEnumerator FlyExit()
    {
        yield return new WaitForSeconds(0.7f);
        isFly = false;
        print("开始下落");
        isDown = true;
        yield return new WaitForSeconds(0.7f);
        isDown = false;

        if (id == BattleFieldManager.Instance.MyPlayerIndex)
        {
            DontPanel.Instance.gameObject.SetActive(false);
        }
        GetComponent<NavMeshAgent>().enabled = true;
        
        print("结束");
    }
    #endregion
}
