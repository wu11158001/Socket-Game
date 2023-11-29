using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public GameObject item;
    public Transform gameInfoListTransform;
    public Text time_Txt;
    public Button exitGame_Btn;

    private float startTime;

    private Dictionary<string, GameInfoList> infoDic = new Dictionary<string, GameInfoList>();

    /// <summary>
    /// UI面板開始
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        Entter();
    }

    /// <summary>
    /// UI面板退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    /// <summary>
    /// UI面板暫停
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    /// <summary>
    /// UI面板繼續
    /// </summary>
    public override void OnRecovery()
    {
        base.OnRecovery();
        Entter();
    }

    /// <summary>
    /// 進入
    /// </summary>
    void Entter()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 離開
    /// </summary>
    void Exit()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        startTime = Time.time;

        exitGame_Btn.onClick.AddListener(OnExitGameClick);
    }

    private void FixedUpdate()
    {
        time_Txt.text = Mathf.Clamp((int)(Time.time - startTime), 0, 300).ToString();
    }

    /// <summary>
    /// 按下離開遊戲
    /// </summary>
    public void OnExitGameClick()
    {

    }
    
    /// <summary>
    /// 更新玩家訊息列表
    /// </summary>
    /// <param name="packs"></param>
    public void UpdateGameInfoList(List<PlayerPack> packs)
    {
        foreach (var player in packs)
        {
            GameObject obj = Instantiate(item);
            obj.transform.SetParent(gameInfoListTransform);
            GameInfoList gameInfoList = obj.GetComponent<GameInfoList>();
            gameInfoList.SetInfo(player.PlayerName, player.HP);
            infoDic.Add(player.PlayerID, gameInfoList);
        }
    }

    /// <summary>
    /// 更新玩家訊息列表內容
    /// </summary>
    /// <param name="id"></param>
    /// <param name="hp"></param>
    public void UpdateGameInfoValue(string id, int hp)
    {
        if(infoDic.TryGetValue(id, out GameInfoList gameInfoList))
        {
            gameInfoList.UpdateValue(hp);
        }
        else
        {
            Debug.LogError("獲取不到對應的角色訊息");
        }
    }
}
