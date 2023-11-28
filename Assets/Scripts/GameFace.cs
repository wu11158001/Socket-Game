using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class GameFace : MonoBehaviour
{
    private static GameFace gameFace;
    public static GameFace Instance 
    { 
        get 
        { 
            if(gameFace == null) gameFace = GameObject.Find("GameFace").GetComponent<GameFace>();
            return gameFace;
        } 
    }

    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uIManager;

    private void Awake()
    {
        uIManager = new UIManager(this);
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);        

        uIManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();        
    }

    /// <summary>
    /// 發送
    /// </summary>
    /// <param name="pack"></param>
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    /// <summary>
    /// 處理回覆
    /// </summary>
    /// <param name="pack"></param>
    public void HandleResponse(MainPack pack)
    {
        requestManager.HandleResponse(pack);
    }

    /// <summary>
    /// 添加請求
    /// </summary>
    /// <param name="request"></param>
    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    /// <summary>
    /// 移除請求
    /// </summary>
    /// <param name="action"></param>
    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    /// <param name="isSync">是否為異步</param>
    public void ShowTip(string str, bool isSync = false)
    {
        uIManager.ShowTip(str, isSync);
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
    }
}
