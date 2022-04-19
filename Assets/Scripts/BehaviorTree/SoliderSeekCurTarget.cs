using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoliderSeekCurTarget : Action
{
    //添加小兵攻击请求CD 时间戳计数
    private float SoliderAttTime=0;
    public float SoliderAttCD=1.5f;
    public int SoliderAtt;
    public override TaskStatus OnUpdate()
    {

        var target = GetSolider().GetCurTarget();
        if ( target != null && target.GetComponent<BodyModel>().isDead == false)
        {
            var dis = Vector3.Distance(GetComponent<Transform>().position, target.transform.position);
            if (dis < 3)
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                if (Time.time > SoliderAttTime)
                {
                    SoliderAttTime = Time.time + SoliderAttCD;
                    //小兵攻击目标
                   
                    GetSolider().GetComponent<Animator>().SetTrigger("attack");
                }
            }
            else
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
            }
            
            GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
    Solider GetSolider()
    {
        return GetComponent<Solider>();
    }
}
