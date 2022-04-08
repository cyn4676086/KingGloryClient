using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;

public abstract  class Request : MonoBehaviour {

	public OperationCode OpCode;
	public abstract void DefaultRequest();
	public void Awake()
    {
		PhotonEngine.Instance.AddRequest(this);
    }
	public void Start()
    {
    
    }
	public void OnDestory()
    {
		PhotonEngine.Instance.RemoveRequest(this);
    }

	public abstract void OnOperationResponse(OperationResponse operationResponse);

	public abstract void OnEvent(EventData data);

}
