using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using UnityEngine.UI;
using System.Threading.Tasks;

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
    public Text time_Txt;
    public Button exitGame_Btn;
    public GameExitRequest gameExitRequest;

    private float startTime;
    private GameObject stage_obj;

    public GameObject gameOver_Obj;
    public Text result_Txt;
    public Button confirm_Btn;

    

    //存放玩家訊息(玩家名稱,訊息列表)
    private Dictionary<string, GameInfoItem> infoDic = new Dictionary<string, GameInfoItem>();
    //存放玩家位置箭頭(玩家狀態,箭頭物件)
    private Dictionary<UpdateCharacterState, RectTransform> playerArrowDic = new Dictionary<UpdateCharacterState, RectTransform>();
    //存放玩家頭頂訊息(玩家名稱,頭頂物件)
    private Dictionary<UpdateCharacterState, RectTransform> headInfoDic = new Dictionary<UpdateCharacterState, RectTransform>();

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

        startTime = Time.time;
        stage_obj = Instantiate(Resources.Load<GameObject>("Prefab/GameScene"));

        exitGame_Btn.onClick.AddListener(OnExitGameClick);
        confirm_Btn.onClick.AddListener(OnExitGameClick);
    }

    private void FixedUpdate()
    {
        time_Txt.text = Mathf.Clamp((int)(Time.time - startTime), 0, 300).ToString();

        foreach (var arrow in playerArrowDic)
        {
            if (arrow.Key != null)
            {
                Vector3 viewportPos = mainCamera.WorldToViewportPoint(arrow.Key.transform.position);

                //判斷是否在視野內
                if ((viewportPos.x > 0 && viewportPos.x < 1) || !arrow.Key.GetActionable)
                {
                    arrow.Value.gameObject.SetActive(false);
                    if(headInfoDic.TryGetValue(arrow.Key, out RectTransform rt))
                    {
                        rt.gameObject.SetActive(false);
                    }
                }
                else
                {
                    //位置箭頭物件
                    arrow.Value.gameObject.SetActive(true);
                    float dir = viewportPos.x < 0 ? -1 : 1;
                    float arrowPosX = ((canvasRect.rect.width / 2) - 50) * dir;
                    float arrowPosY = (canvasRect.rect.height * viewportPos.y) - (canvasRect.rect.height / 2);
                    float arrowRotZ = viewportPos.x < 0 ? 180 : 0;
                    arrow.Value.rotation = Quaternion.Euler(0, 0, arrowRotZ);
                    arrow.Value.anchoredPosition = new Vector2(arrowPosX, arrowPosY);

                    //頭頂訊息物件
                    if (headInfoDic.TryGetValue(arrow.Key, out RectTransform rt))
                    {
                        rt.gameObject.SetActive(true);
                        float headInfoPosX = (canvasRect.rect.width * viewportPos.x) - (canvasRect.rect.width / 2);
                        float headInfoPosY = (canvasRect.rect.height / 2 * viewportPos.y);
                        rt.anchoredPosition = new Vector2(headInfoPosX, headInfoPosY);
                    }              
                }
            }            
        }
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
        //移除位置箭頭物件
        foreach (var arrow in playerArrowDic.Values)
        {
            Destroy(arrow.gameObject);
        }
        playerArrowDic.Clear();

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
            playerArrowDic.Add(gameFace.GetPlayers()[player.PlayerName], arrowObj);

            //添加頭頂訊息
            RectTransform headInfoObj = Instantiate(headInfo).GetComponent<RectTransform>();
            headInfoObj.transform.SetParent(headInfoTransform);
            headInfoObj.name = $"{player.PlayerName}_HeadInfo";
            Image healthBar = headInfoObj.transform.Find("HealthBar_Img").GetComponent<Image>();
            healthBar.fillAmount = player.HP / player.MaxHp;
            headInfoDic.Add(gameFace.GetPlayers()[player.PlayerName], headInfoObj);
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
        result_Txt.text = result ? "獲勝!!!" : "死掉了...";
    }
}
