using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldDataManager : MonoBehaviour {
    public static BattleFieldDataManager Instance;
    // Use this for initialization
    void Start () {
        if (Instance == null)
        {
            Instance = this;
        }
        BgTime = Time.time;

        TowerDataManager();
    }
	
	// Update is called once per frame
	void Update () {
        BFTime = Time.time - BgTime;
	}
	public float BFTime;
	private float BgTime;
    #region 炮塔管理 

    public int TowerHurtAdd;
    public int TowerMaxHurt;
    private void TowerDataManager()
    {
        StartCoroutine(Tower());
    }

    private IEnumerator Tower()
    {
        //炮塔伤害每分钟增加的值 加负数
        while (true)
        {
            yield return new WaitForSeconds(60f);
            GameObject.Find("TowerR1").GetComponent<TowerManager>().attVal = 
                GameObject.Find("TowerR1").GetComponent<TowerManager>().attVal + TowerHurtAdd;

            GameObject.Find("TowerB1").GetComponent<TowerManager>().attVal =
               GameObject.Find("TowerB1").GetComponent<TowerManager>().attVal + TowerHurtAdd;

           if (GameObject.Find("TowerR1").GetComponent<TowerManager>().attVal >= TowerMaxHurt)
            {
                break;
            }
        }
    }
    #endregion
    #region 小兵管理见SoliderSpwan
    #endregion
    #region 英雄管理
    #endregion
}

