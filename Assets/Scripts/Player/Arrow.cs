using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	internal int Owner;
	internal BodyModel target;
	private float speed = 10f;
    [HideInInspector]
    public int ArrowHurt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //判断空
        
        MonoBehaviour obj = target as MonoBehaviour;
        if (obj)
        {
            transform.position = Vector3.Lerp(transform.position, obj.transform.position + Vector3.up, speed * Time.deltaTime);

            transform.LookAt(obj.transform.position + Vector3.up);
            //每次移动的时候，判断和目标的距离。如果距离小于1，则是碰到了
            var dis = Vector3.Distance(transform.position, obj.transform.position + Vector3.up);
            if (dis < 1)
            {
                //print("检测到碰撞，销毁箭");
                Destroy(this.gameObject);
                //发送协议
                if (target.isDead!=true&& Owner == BattleFieldManager.Instance.MyPlayerIndex)
                {
                    target.SendHurtRequest(ArrowHurt, Owner);
                }
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
