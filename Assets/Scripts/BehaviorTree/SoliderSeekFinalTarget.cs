using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoliderSeekFinalTarget : Action
{
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        if (GetSolider().FinalTarget() != null)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(GetSolider().FinalTarget().transform.position);
            //动画
            GetSolider().GetComponent<Animator>().SetFloat("speed", GetComponent<NavMeshAgent>().velocity.magnitude);
            //Debug.LogError("移动到终极目标");
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
    Solider GetSolider()
    {
        return GetComponent<Solider>();
    }
}
