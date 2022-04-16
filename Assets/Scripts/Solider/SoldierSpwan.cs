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
        yield return new WaitForSeconds(4f);
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                var s = Instantiate(SodierPF,transform.position, Quaternion.identity);
                s.GetComponent<NavMeshAgent>().enabled = true;
                //print(s + ":" + s.GetComponent<Soldier>());
                s.GetComponent<Solider>().Init(FinalTargetList, Group, 6000 + index + Group * 100);
                index++;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(25f);
        }
    }

}
