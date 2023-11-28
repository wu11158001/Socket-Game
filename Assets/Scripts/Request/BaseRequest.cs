using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;

    protected ActionCode actionCode;
    public ActionCode GetActionCode { get { return actionCode; } }

    protected GameFace gameFace;

    public virtual void Awake()
    {
        gameFace = GameFace.Instance;
    }

    public virtual void Start()
    {
        gameFace.AddRequest(this);
        Debug.Log("添加:" + actionCode.ToString());
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="pack"></param>
    public virtual void OnResponse(MainPack pack)
    {

    }

    /// <summary>
    /// 發送請求
    /// </summary>
    /// <param name="pack"></param>
    public virtual void SendRequest(MainPack pack)
    {
        gameFace.Send(pack);
    }

    public virtual void OnDestroy()
    {
        gameFace.RemoveRequest(actionCode);
    }
}
