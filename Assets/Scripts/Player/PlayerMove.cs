﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{

    #region 生命周期
    
    public bool isAttacking = false;
    public float Speed=1;

    void Update()
    {
        SendMyAxis();
        Move(DirX, DirY);
    }
    #endregion
    #region 移动
    internal short DirX;
    internal short DirY;
    private float rotationSpeed = 10;
    

    public PlayerModel Model { get { return GetComponent<PlayerModel>(); } }
    public void Move(int x, int y)
    {
        var speed = new Vector3(x, 0, y);
        if (isAttacking == true)
        {
            return;
        }
        //控制 nav agent移动
        GetComponent<NavMeshAgent>().velocity = new Vector3(x, 0, y) * 3f*Speed;
        //切换动画状态机
        GetComponent<Animator>().SetFloat ("speed", speed.magnitude);
        //速度小于0的时候，就不旋转。
        if (speed.magnitude == 0)
        {
            return;
        }
        //旋转
        Quaternion targetRotion;
        targetRotion = Quaternion.LookRotation(GetComponent<Transform>().position + speed - transform.position);
        if (targetRotion != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotion, rotationSpeed * Time.deltaTime);
        }

    }
    private void SendMyAxis()
    {
        //把指令发送给服务器
        if (GetComponent<PlayerModel>().isMe)
        {
            if (Model.Group == 1)
            {
                //获取输入轴 
                int posX = (int)Math.Round(ETCInput.GetAxis("Horizontal"));
                int posY = (int)Math.Round(ETCInput.GetAxis("Vertical"));
                //是自己直接移动不需要等待服务器返回
                BattleFieldManager.Instance.MovePlayer((short)GetComponent<PlayerModel>()
                    .id, (short)posX, (short)posY, transform.position.x, transform.position.y, transform.position.z);
                BattleFieldRequest.Instance.MoveRequest(posX, posY, transform.position);
            }
            else if (Model.Group == 2)
            {
                //获取输入轴 
                int posX = -(int)Math.Round(ETCInput.GetAxis("Horizontal"));
                int posY = -(int)Math.Round(ETCInput.GetAxis("Vertical"));
                //是自己直接移动不需要等待服务器返回
                BattleFieldManager.Instance.MovePlayer((short)GetComponent<PlayerModel>()
                    .id, (short)posX, (short)posY, transform.position.x, transform.position.y, transform.position.z);
                BattleFieldRequest.Instance.MoveRequest(posX, posY, transform.position);
            }
        }
    }
   

    

    #endregion

}