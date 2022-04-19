using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierSpwan : MonoBehaviour
{
    //保存协程，用于稍后关闭
    private Coroutine Coroutine;
    public GameObject SodierPF;
    public List<GameObject> FinalTargetList;

    public int Group;
    private int index;
    private int flag = 0;
    private void OnEnable()
    {
        //开始生成小兵
        BattleFieldManager.Instance.OnInitCallBack += StartSpwan;
    }

    private void StartSpwan()
    {
        Coroutine = StartCoroutine(SpwanSolider());
    }

    private void OnDisable()
    {
        //关闭协程
        StopCoroutine(Coroutine);
    }

    private IEnumerator SpwanSolider()
    {
        yield return new WaitForSeconds(6f);
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                var s = Instantiate(SodierPF,transform.position, Quaternion.identity);
                s.GetComponent<NavMeshAgent>().enabled = true;
                s.GetComponent<Solider>().Init(FinalTargetList, Group, 6000 + index + Group * 100);
                SoliderDataManager(s);
                print(flag);
                print(s.GetComponent<Solider>().HP + " "+s.GetComponent<Solider>().SoliderAtt);
                index++;
                yield return new WaitForSeconds(0.6f);
            }
            ++flag;
            yield return new WaitForSeconds(43f);
        }
    }

    #region 小兵数据管理
    private void SoliderDataManager(GameObject solider)
    {
        float time = BattleFieldDataManager.Instance.BFTime;
        if (time < 180)
        {
            solider.GetComponent<Solider>().HP = solider.GetComponent<Solider>().HP+55*flag;
            solider.GetComponent<Solider>().SoliderAtt = solider.GetComponent<Solider>().SoliderAtt-flag;
        }
        if (flag >= 3 && time >= 180)
        {
            flag = 0;
        }
        if (time >= 180&&time<300)
        {
            solider.GetComponent<Solider>().HP = 1901;
            solider.GetComponent<Solider>().HP = solider.GetComponent<Solider>().HP + 70 * flag;
            solider.GetComponent<Solider>().SoliderAtt = -120;
            solider.GetComponent<Solider>().SoliderAtt = solider.GetComponent<Solider>().SoliderAtt - flag;
        }
        if (flag >= 8 && time >= 300 && time < 600)
        {
            flag = 0;
        }
        if (time >= 300&&time<600)
        {
            solider.GetComponent<Solider>().HP = 2536;
            solider.GetComponent<Solider>().HP = solider.GetComponent<Solider>().HP + 90 * flag;
            solider.GetComponent<Solider>().SoliderAtt = -213;
            solider.GetComponent<Solider>().SoliderAtt = solider.GetComponent<Solider>().SoliderAtt -2* flag;
        }
        if (flag >= 8  && time >= 600)
        {
            flag = 0;
        }
        if (time >= 600)
        {
            solider.GetComponent<Solider>().HP = 3192;
            solider.GetComponent<Solider>().HP = solider.GetComponent<Solider>().HP + 120 * flag;
            solider.GetComponent<Solider>().SoliderAtt = -243;
            solider.GetComponent<Solider>().SoliderAtt = solider.GetComponent<Solider>().SoliderAtt - 3 * flag;
        }
    }
    #endregion

}
