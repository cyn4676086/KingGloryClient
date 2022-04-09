using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    internal GameObject target;
    //Action<GameObject>：这个变量是一个方法，并且这个方法是具有一个参数Gameobject类型。
    //委托
    public Action<GameObject> TouchCallback;
    void Start()
    {

    }

    void Update()
    {
        if (target.GetComponent<BodyModel>().isDead != true && target != null)
        {
            //移动
            //transform.LookAt(target.transform.position);
            var dis = target.transform.position - transform.position;
            transform.Translate(dis.normalized * 8f * Time.deltaTime);
            //碰撞检查
            if (dis.magnitude < 0.3f)
            {
                TouchCallback(target);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

}

