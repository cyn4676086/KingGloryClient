using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderCanSeeEnemy : Conditional {
    public Transform[] targets;//判断是否在视野内的目标
    public float fieldOfView = 360;
    public SharedFloat aharedViewDistance;

    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (GetSolider().GetCurTarget() == null)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
    Solider GetSolider()
    {
        return GetComponent<Solider>();
    }
}
