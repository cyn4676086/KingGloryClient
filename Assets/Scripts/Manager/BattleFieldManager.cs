using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public static BattleFieldManager Instance;
    public int MyPlayerIndex = -1;

    public GameObject Hero;
    public List<Transform> HeroPos;

    [HideInInspector]
    public List<BodyModel> playerList = new List<BodyModel>();
    [HideInInspector]
    public List<BodyModel> TowerList = new List<BodyModel>();
    [HideInInspector]
    public List<BodyModel> SoliderList = new List<BodyModel>();
    
    public PlayerMove Player;
    public GameObject attFx;
    public Action OnInitCallBack;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   
    /// <summary>
    /// 人物生成逻辑
    /// </summary>
    /// <param name="playerIndex"></param>
    internal void InitBattleField(int playerIndex)
    {
        //初始化场景
        MyPlayerIndex = playerIndex;
        //开始实例化
        Player = (Instantiate(Hero, HeroPos[MyPlayerIndex - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
        Player.Model.id = MyPlayerIndex;
        Player.Model.isMe = true;
        Player.Model.Group = MyPlayerIndex;
        Player.Model.isDead = false;
        playerList.Add(Player.Model);
        Player.GetComponent<BodyModel>().SetHealth();
        Camera.main.GetComponent<CameraFollow>().Init(Player.gameObject);
        OnInitCallBack();

    }

    internal void AddPlayer(string allPlayer)
    {
        //解析
        var arr = allPlayer.Split(',');
        foreach (var item in arr)//遍历在场玩家的index
        {
            try//最后多了一个空格，所以转换成int会失败，不过这里忽略不计
            {
                //转换成int
                int i = Convert.ToInt16(item);

                if (i != MyPlayerIndex)
                {
                   
                    PlayerMove p = (Instantiate(Hero, HeroPos[i - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
                    p.Model.id = i;
                    p.Model.Group = i;
                    p.Model.isDead = false;
                    playerList.Add(p.Model);
                    p.GetComponent<BodyModel>().SetHealth();
                }
            }
            catch (Exception)
            {

            }

        }
    }

    internal void AddOnePlayer(int index)
    {
        //解析
        try//最后多了一个空格，所以转换成int会失败，不过这里忽略不计
        {
            if (index != MyPlayerIndex)
            {
               
                PlayerMove p = (Instantiate(Hero, HeroPos[index - 1].position, Quaternion.identity) as GameObject).GetComponent<PlayerMove>();
                p.Model.id = index;
                p.Model.Group = index;
                playerList.Add(p.Model);
                p.GetComponent<BodyModel>().SetHealth();
            }
        }
        catch (Exception)
        {

        }

    }

    /// <summary>
    /// 收到服务器发送的移动数据，进行解析显示
    /// </summary>
    internal void MovePlayer(short index, short x, short y, float Px, float Py, float Pz)
    {
        //使用向量提前量来抵消与服务端的延迟
        foreach (var item in playerList)
        {
            //拿到对应index的对象
            if (item.id == index)
            {
                var move = item.GetComponent<PlayerMove>();
                //设置移动方向
                move.DirX = x;
                move.DirY = y;
                item.transform.position = new Vector3(Px, Py, Pz);
            }
        }
    }

    
    internal PlayerMove GetPlayerByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in playerList)
        {
            if (item.id == attIndex)
            {
                return item.GetComponent<PlayerMove>();
            }
        }
        return null;
    }

    internal TowerManager GetTowerByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in TowerList)
        {
            if (item.id == attIndex)
            {
                return item.GetComponent<TowerManager>();
            }
        }
        return null;
    }
    internal Solider GetSoliderByID(int attIndex)
    {
        //通过id找到对象
        foreach (var item in SoliderList)
        {
            if (item.id == attIndex&&item!=null)
            {
                return item.GetComponent<Solider>();
            }
        }
        return null;
    }

    internal void PlayAtt(int player, int target, int type)
    {

       // print("PlayAtt:" + player + "type:" + type);
        var item = GetPlayerByID(player);

        if (type == (int)Common.AttackType.HanBingNormal)
        {
            //寒冰 普通攻击动画
            item.GetComponent<HanBingAtt>().PlayAtt(target);
        }
        else if (type == (int)Common.AttackType.HanBingSkill1)
        {
            //寒冰 技能1攻击动画
            item.GetComponent<HanBingAtt>().PlaySkill(target);
        }
        else if (type == (int)Common.AttackType.HanBingSkill2)
        {
            //寒冰 技能2攻击动画
            item.GetComponent<HanBingAtt>().PlaySkill2();
        }
    }
    internal void TowerDestory(int index,int exp,int objectID)
    {
        if (Mathf.Round(index / 1000) == 2)
        {
            //防御塔
           // print("销毁" + index);
            var item = GetTowerByID(index);
            item.PlayDestroy();
        }
        //print("ObjID" + objectID);
        if(Mathf.Round(objectID / 1000) == 0)
        {
            var item = GetPlayerByID(objectID);
            item.Model.ExpUp(exp);
        }
    }
    //接收服务器发送的伤害数据
    internal void Hurt(int index, int hp,int ObjectID)
    {
        if (Mathf.Round(index / 1000) == 2)
        {
            //塔被扣血
            var item = GetTowerByID(index);
            item.HP += hp;
           // print(index + " 被攻击，剩余血量 " + item.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);

            if (item.HP <= 0)
            {
                //print("塔爆炸");
                BattleFieldRequest.Instance.DestoryRequest(item.id,100,ObjectID);
                if (item.id % 2 == 0)
                {
                    BattleFieldRequest.Instance.EndingRequest(item.Group);
                }
            }
            ShowBlood(item.gameObject,hp);
        }
        else if(Mathf.Round(index / 1000) == 0)
        {
            //角色被扣血
            var item = GetPlayerByID(index);
            item.Model.HP += hp;
            //print(index + " 被攻击，剩余血量 " + item.Model.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);
            ShowBlood(item.gameObject, hp);
            if (item.Model.HP <= 0)
            {
                //角色死亡，进入复活倒计时
                item.PlayerDead();
            }
            
        }
        else if(Mathf.Round(index / 1000) == 6)
        {
            //小兵被扣血
            var item = GetSoliderByID(index);
            item.HP += hp;
            //print(index + " 被攻击，剩余血量 " + item.HP);
            //播放特效fx
            Instantiate(attFx, item.transform.position + Vector3.up, Quaternion.identity);

            if (item!=null&&item.HP <= 0)
            {
                //print("小兵dead");
                item.SoliderDeathAnimator();
            }
            ShowBlood(item.gameObject, hp);
        }
    }
   
    //扣血动画
    private void ShowBlood(GameObject item,int hp)
    {
        //Build the information
        HUDTextInfo info = new HUDTextInfo(item.transform, string.Format("{1}{0}", hp, ""));

        info.Color = (hp > 0 ? Color.green:Color.red) ;
        info.Size = UnityEngine.Random.Range(30, 60);
        info.Speed = UnityEngine.Random.Range(0.5f, 1);
        info.VerticalAceleration = UnityEngine.Random.Range(-2, 2f);
        info.VerticalPositionOffset = 2f;
        info.VerticalFactorScale = UnityEngine.Random.Range(1.2f, 10);
        info.Side = (UnityEngine.Random.Range(0, 2) == 1) ? bl_Guidance.LeftDown : bl_Guidance.RightDown;
        info.ExtraDelayTime = -1;
        info.AnimationType = bl_HUDText.TextAnimationType.PingPong;
        info.FadeSpeed = 200;
        info.ExtraFloatSpeed = -11;
        //Send the information
        bl_UHTUtils.GetHUDText.NewText(info);
    }

    public void OnAttBtn()
    {
        //调用P'layer'Move.OnAttBtn()
        Player.GetComponent<HanBingAtt>().OnAttBtn();
    }
    public void OnSkill1()
    {
        Player.GetComponent<HanBingAtt>().OnSkill1();
    }
    public void OnSkill2()
    {
        Player.GetComponent<HanBingAtt>().OnSkill2();
    }
}
