using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceController : MonoBehaviour {
    public int FreezeHurt;
    void Start () {
        FreezeHurt = HanBingAtt.instance.Skill3Hurt;
	}
	
	void Update () {
		
	}
    private int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>() == null && i>0)
        {
            return;
        }
        other.gameObject.GetComponent<PlayerModel>().FreezeEnter(FreezeHurt);
        i++;
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
}
