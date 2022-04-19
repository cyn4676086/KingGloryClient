using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthWater : MonoBehaviour {
    
    public int group;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>() == null)
        {
            return;
        }
        if(other.GetComponent<PlayerModel>().Group==group&& other.GetComponent<PlayerModel>().isDead != true)
        {
            cor= StartCoroutine(Cure(other));
        }
    }
    private Coroutine cor;
    private void OnTriggerExit(Collider other)
    {
        if (cor != null)
        {
            StopCoroutine(cor);
        }
        
    }
    
    private IEnumerator Cure(Collider other)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if(other.GetComponent<PlayerModel>().HP < other.GetComponent<HealthBar>().defaultHealth-1)
            {
                BattleFieldRequest.Instance.HurtRequest(other.GetComponent<PlayerModel>().id, 1500, other.GetComponent<PlayerModel>().id);
            }

            if (other.GetComponent<PlayerModel>().HP ==other.GetComponent<HealthBar>().defaultHealth - 1)
            {
                StopCoroutine(cor);
                break;
            }
        }
    }
}
