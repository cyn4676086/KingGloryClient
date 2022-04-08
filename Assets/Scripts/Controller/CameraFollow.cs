using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject target;
    private Vector3 offsetRed = new Vector3(0f,12f,-10f);
    private Vector3 offsetBlue = new Vector3(0f, 12f,10f);
    private Vector3 offset;

    void Start()
    {
        
    }
    public void Init(GameObject target)
    {
        this.target = target;
        if (target.GetComponent<BodyModel>().Group == 1)
        {
            offset = offsetRed;
            this.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
        }else if (target.GetComponent<BodyModel>().Group == 2)
        {
            offset = offsetBlue;
            this.transform.rotation = Quaternion.Euler(50f, 180f, 0f);
        }
        
    }
    void Update()
    {
        if (target!=null)
        {
            this.transform.position = target.transform.position + offset;
        }
    }
}