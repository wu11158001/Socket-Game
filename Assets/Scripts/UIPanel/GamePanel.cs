using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public GameObject gameInfoItem;
    public Transform gameInfoListTransform;
    public Text time_Txt;
    public Button exitGame_Btn;
    public GameExitRequest gameExitRequest;

    private float startTime;
    private GameObject stage_obj;

    //存放玩家訊息
    private Dictionary<string, GameInfoItem> infoDic = new Dictionary<string, GameInfoItem>();

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
        if(stage_obj != null) stage_obj.SetActive(true);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 離開
    /// </summary>
    void Exit()
    {
        stage_obj.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        exitGame_Btn.onClick.AddListener(OnExitGameClick);

        startTime = Time.time;
        stage_obj = Instantiate(Resources.Load<GameObject>("Prefab/GameScene"));
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
        gameExitRequest.SendRequest();
        gameFace.LeaveGame();
    }

    /// <summary>
    /// 更新玩家訊息列表
    /// </summary>
    /// <param name="pack"></param>
    public void UpdateGameInfoList(MainPack pack)
    {
        //移除訊息項目
        for (int i = 0; i < gameInfoListTransform.childCount; i++)
        {
            Destroy(gameInfoListTransform.GetChild(i).gameObject);
        }
        infoDic.Clear();

        //添加訊息項目
        foreach (var player in pack.PlayerPack)
        {
            GameObject obj = Instantiate(gameInfoItem);
            obj.transform.SetParent(gameInfoListTransform);
            GameInfoItem infoItem = obj.GetComponent<GameInfoItem>();
            infoItem.SetInfo(player.PlayerName, player.HP);
            infoDic.Add(player.PlayerName, infoItem);
        }
    }

    /// <summary>
    /// 更新玩家訊息列表內容
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="hp"></param>
    public void UpdateGameInfoValue(string userName, int hp)
    {
        if(infoDic.TryGetValue(userName, out GameInfoItem gameInfoItem))
        {
            gameInfoItem.UpdateHPValue(hp);
        }
        else
        {
            Debug.LogError("獲取不到對應的角色訊息");
        }
    }
}
