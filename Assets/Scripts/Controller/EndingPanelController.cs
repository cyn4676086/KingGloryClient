using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingPanelController : MonoBehaviour
{
    public static EndingPanelController instance;
    private GameObject camera;
    private void Awake()
    {
        instance = this;
        //刚开始应该隐藏
        this.gameObject.SetActive(false);
    }

    public void Ending(int id)
    {
        
        //结算页面显示出来
        this.gameObject.SetActive(true);
        var zi = id == BattleFieldManager.Instance.MyPlayerIndex ? "失败" : "胜利";
        transform.Find("Text").GetComponent<Text>().text = zi;

        foreach (var item in BattleFieldManager.Instance.playerList)
        {
            //禁止操作角色
            item.GetComponent<PlayerMove>().enabled = false;
        }
        //销毁
        Destroy(BattleFieldManager.Instance);
    }
    /// <summary>
    /// 当退出按钮按下 返回大厅
    /// </summary>
    public void Quit()
    {
        SceneManager.LoadScene("GameHall");
    }
    //public void Quit()
    //{
    //    transform.parent.Find("MatchingPanel").gameObject.SetActive(true);
    //    this.gameObject.SetActive(false);

    //    //销毁角色 创建新的比赛
    //    foreach (var item in BattleFieldManager.Instance.playerList)
    //    {
    //        Destroy(item.gameObject);
    //        BattleFieldManager.Instance.playerList = new List<BodyModel>();
    //    }
    //}
    
}