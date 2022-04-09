using System.Collections.Generic;
using UnityEngine;

public class TowerManager : BodyModel, IHurtObject
{
    private List<GameObject> targets = new List<GameObject>();
    public GameObject fireFB;
    private GameObject fire;

    private GameObject guanghuan;
    private GameObject TowerDeath;
    private LineRenderer Line;

    public GameObject Tower3DModel;
    public int attVal;

    
    void Start()
    {
        //注册自己 添加到list
        BattleFieldManager.Instance.TowerList.Add(this);
        Line = transform.Find("Line").gameObject.GetComponent<LineRenderer>();
        guanghuan = transform.Find("guangquan_jianta").gameObject;
        guanghuan.SetActive(false);
        TowerDeath = transform.Find("TowerDeathA").gameObject;
    }
    
    void Update()
    {
        if (GetTarget() != null)
        {
            Line.gameObject.SetActive(true);
            Line.SetPosition(0, transform.position + new Vector3(0, 5.6f, 0));
            Line.SetPosition(1, GetTarget().transform.position + new Vector3(0, 1.5f, 0));
        }
        else
        {
            Line.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BodyModel>() == null)
        {
            return;
        }

        //判断进来的人，是不是敌人
        if (isDead || Group == other.GetComponent<BodyModel>().Group)
        {
            return;
        }
        targets.Add(other.gameObject);
        //只有添加第一个对象的时候，才攻击
        if (targets.Count == 1)
        {
            Attack();
        }

        //显示攻击范围,只有player1才显示
        if (GetTarget() == BattleFieldManager.Instance.Player.gameObject && other.GetComponent<PlayerModel>() != null)
        {
            guanghuan.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (targets.IndexOf(other.gameObject) >= -1)
        {
            targets.Remove(other.gameObject);
        }
        if (other.gameObject == BattleFieldManager.Instance.Player.gameObject)
        {
            guanghuan.SetActive(false);
        }
    }

    private void Attack()
    {
        if (isDead || GetTarget() == null) { return; }
        //创建火球
        fire = Instantiate(fireFB, transform.position + new Vector3(0, 5.6f, 0), Quaternion.identity);
        fire.GetComponent<FireBallMove>().target = this.GetTarget();
        fire.GetComponent<FireBallMove>().TouchCallback = OnTouchCallback;
    }

    private void OnTouchCallback(GameObject obj)
    {
        //再进行攻击
        if (GetTarget() != null)
        {
            if (GetTarget().GetComponent<BodyModel>().HP > attVal)
            {
                Attack();
            }
            else if (targets.Count >= 2)
            {
                //如果这次能够打死他，直接从目标中移除，打下一个
                targets.Remove(targets[0]);
                Attack();
            }//如果打死，并且没有下一个目标了，不攻击
        }

        //判断被打中的人，是不是自己
        if (BattleFieldManager.Instance.Player.Model.Group
            != obj.GetComponent<BodyModel>().Group)
        {
            return;
        }
        print("喷到了，要做一些什么呢");
        BattleFieldRequest.Instance.HurtRequest(obj.GetComponent<BodyModel>().id, attVal, id);
    }

    public override void SendHurtRequest(int hurtValue, int ObjectID)
    {
        BattleFieldRequest.Instance.HurtRequest(id, hurtValue, ObjectID);
    }

    internal void PlayDestroy()
    {
        var offset = new Vector3(0f, -3f, 0f);
        TowerDeath.GetComponent<ParticleSystem>().Play();
        //Tower3DModel.transform.Translate(Vector3.up * 3);
        Tower3DModel.transform.position = Tower3DModel.transform.position + offset;
        enabled = false;
        Line.gameObject.SetActive(false);
        isDead = true;
        Destroy(guanghuan);
    }
    GameObject GetTarget()
    {
        if (targets.Count == 0)
            return null;
        if (targets[0] == null || targets[0].GetComponent<BodyModel>().isDead == true)
        {
            targets.Remove(targets[0]);
            if (targets.Count == 0)
                return null;
        }
        return targets[0];
    }
}