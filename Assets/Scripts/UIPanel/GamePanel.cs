using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private Camera mainCamera;
    private RectTransform canvasRect;
    [SerializeField] private Transform posArrowTransform;
    [SerializeField] private GameObject posArrow;

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

    

    //存放玩家訊息(名稱,訊息列表)
    private Dictionary<string, GameInfoItem> infoDic = new Dictionary<string, GameInfoItem>();
    //存放玩家誤置箭頭(玩家狀態,箭頭物件)
    private Dictionary<UpdateCharacterState, RectTransform> playerArrowDic = new Dictionary<UpdateCharacterState, RectTransform>();

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

        //判斷其他玩家位置
        foreach (var arrow in playerArrowDic)
        {
            if (arrow.Key != null)
            {
                Vector3 viewportPos = mainCamera.WorldToViewportPoint(arrow.Key.transform.position);
                //判斷是否在視野內
                if (viewportPos.x > 0 && viewportPos.x < 1)
                {
                    arrow.Value.gameObject.SetActive(false);
                }
                else
                {
                    arrow.Value.gameObject.SetActive(true);
                    float dir = viewportPos.x < 0 ? -1 : 1;
                    float posX = ((canvasRect.rect.width / 2) - 50) * dir;
                    float posY = (canvasRect.rect.height * viewportPos.y) - (canvasRect.rect.height / 2);
                    float rotY = viewportPos.x < 0 ? 180 : 0;

                    arrow.Value.rotation = Quaternion.Euler(0, rotY, 0);
                    arrow.Value.anchoredPosition = new Vector2(posX, posY);
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
            GameObject obj = Instantiate(gameInfoItem);
            obj.transform.SetParent(gameInfoListTransform);
            GameInfoItem infoItem = obj.GetComponent<GameInfoItem>();
            infoItem.SetInfo(player.PlayerName, player.HP);
            infoDic.Add(player.PlayerName, infoItem);

            //添加位置箭頭物件
            RectTransform arrow = Instantiate(posArrow).GetComponent<RectTransform>();
            arrow.transform.SetParent(posArrowTransform);
            playerArrowDic.Add(gameFace.GetPlayers()[player.PlayerName], arrow);
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
