using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour {
    protected CanvasGroup canvas;



    public virtual  void OnEnter()
    {
        if (canvas == null)
        {
            canvas = GetComponent<CanvasGroup>();
        }
      
        //print("open");
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
    }

    public virtual void OnExit()
    {
        //print("close");
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
    }

    public virtual void OnPause()
    {
        //print("OnPause");
    }
    public virtual void OnResume()
    {
        //print("OnResume");
    }
}
