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
            if (gameFace == null) gameFace = GameObject.Find("GameFace").GetComponent<GameFace>();
            return gameFace;
        }
    }

    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uIManager;
    private PlayerManager playerManager;

    public string UserName { get; set; }

    //背景
    [SerializeField] private GameObject bg;
    public bool SetBgActive{set{ bg.SetActive(value); }}

    private void Awake()
    {
        uIManager = new UIManager(this);
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        playerManager = new PlayerManager(this);

        uIManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        playerManager.OnInit();
    }

    /// <summary>
    /// 發送TCP
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

    /// <summary>
    /// 添加遊戲玩家
    /// </summary>
    /// <param name="pack"></param>
    public void AddPlayer(MainPack pack)
    {
        playerManager.AddPlayer(pack);
    }

    /// <summary>
    /// 移除遊戲玩家
    /// </summary>
    /// <param name="name"></param>
    public void RemovePlayer(string name)
    {
        playerManager.RemovePlayer(name);
    }

    /// <summary>
    /// 獲取遊戲玩家
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, UpdateCharacterState> GetPlayers()
    {
        return playerManager.GetPlayers;
    }

    /// <summary>
    /// 客戶端離開遊戲
    /// </summary>
    public void LeaveGame()
    {
        SetBgActive = true;
        playerManager.LeaveGame();
        uIManager.PopPanel();
        uIManager.PopPanel();
    }

    /// <summary>
    /// 更新位置
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePos(MainPack pack)
    {
        playerManager.UpdatePos(pack);
    }

    /// <summary>
    /// 更新動畫
    /// </summary>
    /// <param name="pack"></param>
    public void UpdateAni(MainPack pack)
    {
        playerManager.UpdateAni(pack);
    }

    /// <summary>
    /// 玩家攻擊
    /// </summary>
    /// <param name="pack"></param>
    public void PlayerAttack(MainPack pack)
    {
        playerManager.PlayerAttack(pack);
    }

    /// <summary>
    /// 遊戲結果
    /// </summary>
    /// <param name="pack"></param>
    public void GameResult(MainPack pack)
    {
        playerManager.GameResult(pack);
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
        playerManager.OnDestroy();
    }
}
