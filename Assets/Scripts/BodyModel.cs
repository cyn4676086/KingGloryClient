using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModel : MonoBehaviour,IHurtObject{
	public bool isDead;
	public int Group;
	public int id;
	public int HP;

    public virtual void SendHurtRequest(int hurtValue, int ObjectID)
    {
        throw new System.NotImplementedException("子类必须重写此受击函数");
    }
}
