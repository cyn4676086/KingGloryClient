using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeiDianSkill2 : MonoBehaviour {
    public float speed=0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private int i;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>() == null){
            return;
        }
        
        if (other.GetComponent<PlayerModel>() != null && i == 0)
        {
            other.gameObject.GetComponent<PlayerModel>().SpeedDownEnter(0.1f, 5f);
            i++;
            print("雷电2技能减速触发");
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
