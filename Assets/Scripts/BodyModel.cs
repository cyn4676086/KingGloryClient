using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModel : MonoBehaviour,IHurtObject{
    [HideInInspector]
	public bool isDead;
    [HideInInspector]
    public int Group;
    [HideInInspector]
    public int id;
	public int HP;
    public RectTransform HealthBarMe;
    public RectTransform HealthBarTarget;
 
   
    public void SetHealth()
    {
        if (BattleFieldManager.Instance.MyPlayerIndex == this.Group)
        {
            this.GetComponent<HealthBar>().HealthbarPrefab = HealthBarMe;
        }
        else
        {
            this.GetComponent<HealthBar>().HealthbarPrefab = HealthBarTarget;
        }
    }

    public virtual void SendHurtRequest(int hurtValue, int ObjectID)
    {
        throw new System.NotImplementedException("子类必须重写此受击函数");
    }

}
