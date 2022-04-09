using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoliderSeekCurTarget : Action
{
    //添加小兵攻击请求CD 时间戳计数
    private float SoliderAttTime;
    public float SoliderAttCD=2f;
    public int SoliderAtt=-10;
    public override TaskStatus OnUpdate()
    {
        var target = GetSolider().GetCurTarget();
        if (target != null)
        {
            var dis = Vector3.Distance(transform.position, target.transform.position);
            //动画
            GetSolider().GetComponent<Animator>().SetFloat("speed",target.transform.position.magnitude);
            
            if (dis < 3)
            {
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
