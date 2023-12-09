using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Threading;

public class GamePanel : BasePanel
{
    private Camera mainCamera;
    private RectTransform canvasRect;
    [SerializeField] private Transform posArrowTransform;
    [SerializeField] private GameObject posArrow;
    [SerializeField] private Transform headInfoTransform;
    [SerializeField] private GameObject headInfo;

    public GameObject gameInfoItem;
    public Transform gameInfoListTransform;
    public Button exitGame_Btn;
    public GameExitRequest gameExitRequest;

    private GameObject stage_obj;

    public GameObject gameOver_Obj;
    public Text result_Txt;
    public Button confirm_Btn;

    [SerializeField] private GameObject manualObj;
    [SerializeField] private Image manualBg_Img;

    //存放玩家訊息(玩家名稱,訊息列表)
    private Dictionary<string, GameInfoItem> infoDic = new Dictionary<string, GameInfoItem>();
    //存放玩家位置箭頭(玩家狀態,箭頭物件)
    private Dictionary<string, RectTransform> playerArrowDic = new Dictionary<string, RectTransform>();
    //存放玩家頭頂訊息(玩家名稱,頭頂物件)
    private Dictionary<string, HeadInfo> headInfoDic = new Dictionary<string, HeadInfo>();

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
        gameOver_Obj.SetActive(false);

        StartCoroutine(nameof(IStartDountDound));
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
        mainCamera = Camera.main;
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        stage_obj = Instantiate(Resources.Load<GameObject>("Prefab/GameScene"));

        exitGame_Btn.onClick.AddListener(OnExitGameClick);
        confirm_Btn.onClick.AddListener(OnExitGameClick);
    }

    private void Update()
    {
        foreach (var item in infoDic)
        {
            if (item.Value != null)
            {
                if (!gameFace.GetPlayers().TryGetValue(item.Key, out AnimationEvent animationEvent)) return;

                Vector3 viewportPos = mainCamera.WorldToViewportPoint(gameFace.GetPlayers()[item.Key].transform.parent.position);

                //判斷是否在視野內 && 是否可行動
                if ((viewportPos.x > 0 && viewportPos.x < 1))
                {
                    //位置箭頭物件
                    playerArrowDic[item.Key].gameObject.SetActive(false);

                    //頭頂訊息物件
                    headInfoDic[item.Key].gameObject.SetActive(true);
                    float headInfoPosX = (canvasRect.rect.width * viewportPos.x) - (canvasRect.rect.width / 2);
                    float headInfoPosY = ((canvasRect.rect.height * viewportPos.y) - (canvasRect.rect.height / 2)) + 25;
                    headInfoDic[item.Key].SetPos(new Vector2(headInfoPosX, headInfoPosY));
                }
                else
                {
                    //位置箭頭物件
                    playerArrowDic[item.Key].gameObject.SetActive(true);
                    float dir = viewportPos.x < 0 ? -1 : 1;
                    float arrowPosX = ((canvasRect.rect.width / 2) - 50) * dir;
                    float arrowPosY = (canvasRect.rect.height * viewportPos.y) - (canvasRect.rect.height / 2);
                    float arrowRotZ = viewportPos.x < 0 ? 180 : 0;
                    playerArrowDic[item.Key].rotation = Quaternion.Euler(0, 0, arrowRotZ);
                    playerArrowDic[item.Key].anchoredPosition = new Vector2(arrowPosX, arrowPosY);

                    //頭頂訊息物件
                    headInfoDic[item.Key].gameObject.SetActive(false);
                }
            }            
        }
    }

    /// <summary>
    /// 開場畫面倒數
    /// </summary>
    IEnumerator IStartDountDound()
    {
        exitGame_Btn.gameObject.SetActive(false);
        manualObj.SetActive(true);

        Color color = new Color(0, 0, 0, 1);
        while (color.a > 0)
        {
            color.a -= 0.4f * Time.deltaTime;
            manualBg_Img.color = color;
            yield return null;
        }

        exitGame_Btn.gameObject.SetActive(true);
        manualObj.SetActive(false);
    }

    /// <summary>
    /// 按下離開遊戲
    /// </summary>
    public void OnExitGameClick()
    {
        gameFace.SoundButtonClick();
        gameFace.PlayBGM("Hall");
        gameExitRequest.SendRequest();                
        gameFace.LeaveGame();
    }

    /// <summary>
    /// 更新玩家訊息列表
    /// </summary>
    /// <param name="pack"></param>
    public void UpdateGameInfoList(MainPack pack)
    {
        //移除項目
        for (int i = 0; i < gameInfoListTransform.childCount; i++)
        {
            Destroy(gameInfoListTransform.GetChild(i).gameObject);
            Destroy(posArrowTransform.GetChild(i).gameObject);
            Destroy(headInfoTransform.GetChild(i).gameObject);
        }
        infoDic.Clear();
        playerArrowDic.Clear();
        headInfoDic.Clear();

        //添加
        foreach (var player in pack.PlayerPack)
        {
            //添加訊息項目
            GameObject infoItemObj = Instantiate(gameInfoItem);
            infoItemObj.transform.SetParent(gameInfoListTransform);
            infoItemObj.name = $"{player.PlayerName}_InfoItem";
            GameInfoItem infoItem = infoItemObj.GetComponent<GameInfoItem>();
            infoItem.SetInfo(player.PlayerName, player.HP, player.TotalKill);
            infoDic.Add(player.PlayerName, infoItem);

            //添加位置箭頭物件
            RectTransform arrowObj = Instantiate(posArrow).GetComponent<RectTransform>();
            arrowObj.transform.SetParent(posArrowTransform);
            arrowObj.name = $"{player.PlayerName}_Arrow";
            playerArrowDic.Add(player.PlayerName, arrowObj);

            //添加頭頂訊息
            HeadInfo headInfoObj = Instantiate(headInfo).GetComponent<HeadInfo>();
            headInfoObj.transform.SetParent(headInfoTransform);
            headInfoObj.name = $"{player.PlayerName}_HeadInfo";
            float hp = (float)player.HP / (float)player.MaxHp;
            string name = player.PlayerName;
            string colorStr = player.PlayerName == gameFace.UserName ? "#69DE3C" : "#FF0003";
            headInfoObj.SetInitInfo(name, colorStr, hp);
            headInfoDic.Add(player.PlayerName, headInfoObj);
        }
    }

    /// <summary>
    /// 更新遊戲訊息
    /// </summary>
    /// <param name="pack"></param>
    public void UpdateGameInfoListValue(MainPack pack)
    {
        foreach (var player in pack.PlayerPack)
        {
            //訊息項目
            infoDic[player.PlayerName].SetInfo(player.PlayerName, player.HP, player.TotalKill);

            //頭頂訊息            
            headInfoDic[player.PlayerName].SetHPValue((float)player.HP / (float)player.MaxHp);
        }
    }

    /// <summary>
    /// 顯示擊殺訊息
    /// </summary>
    /// <param name="pack"></param>
    async public void ShowKillInfo(MainPack pack)
    {
        string attacker = pack.PlayerPack[0].KillInfoPack.Attacker;

        //更新訊息表
        if (infoDic.ContainsKey(attacker))
        {
            infoDic[attacker].AddKillCount();
        }

        //顯示擊殺訊息       
        foreach (var player in pack.PlayerPack[0].KillInfoPack.DeadList)
        {
            uiManager.ShowTip($"{attacker} 擊殺了 {player}");
            await Task.Delay(3500);
        }  
    }

    /// <summary>
    /// 設定遊戲結果
    /// </summary>
    /// <param name="pack"></param>
    public void SetGameOverInfo(MainPack pack)
    {
        bool result = pack.ReturnCode == ReturnCode.Succeed ? true : false;

        gameOver_Obj.SetActive(true);
        result_Txt.text = result ? "手起刀落!!!" : "死掉了。。。";
    }
}
