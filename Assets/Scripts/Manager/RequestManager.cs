using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class RequestManager : BaseManager
{
    public RequestManager(GameFace gameFace) : base(gameFace) { }

    private Dictionary<ActionCode, BaseRequest> requsetDic = new Dictionary<ActionCode, BaseRequest>();

    /// <summary>
    /// 添加請求
    /// </summary>
    /// <param name="request"></param>
    public void AddRequest(BaseRequest request)
    {
        requsetDic.Add(request.GetActionCode, request);
    }

    /// <summary>
    /// 移除請求
    /// </summary>
    public void RemoveRequest(ActionCode action)
    {
        requsetDic.Remove(action);
    }

    /// <summary>
    /// 處理回覆
    /// </summary>
    /// <param name="pack"></param>
    public void HandleResponse(MainPack pack)
    {
        if (requsetDic.TryGetValue(pack.ActionCode, out BaseRequest request))
        {
            request.OnResponse(pack);
        }
        else
        {
            Debug.LogWarning("不能找到對應的處理");
        }
    }
}
