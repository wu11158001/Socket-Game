using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtobuf;

public class RoomPanel : BasePanel
{
    public Button leave_Btn, send_Btn, start_Btn;
    public InputField chat_IF;
    public Text roomName_Txt, statr_Txt;
    public Scrollbar chat_Sb;
    public Transform userList;
    public GameObject userItemObj;
    public RoomExitRequest roomExitRequest;

    public Text chat_Txt;

    public ChatRequest chatRequest;
    public UpdateRoomUserInfoRequest updateRoomUserInfoRequest;
    public StartGameRequest startGameRequest;

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
        leave_Btn.enabled = true;
        chat_Txt.text = "";
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
        leave_Btn.onClick.AddListener(OnLeaveClick);
        send_Btn.onClick.AddListener(OnSendClick);
        start_Btn.onClick.AddListener(OnStartClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (chat_IF.isFocused) OnSendClick();
        }
    }

    /// <summary>
    /// 按下離開房間
    /// </summary>
    void OnLeaveClick()
    {
        gameFace.ButtonClick();
        roomExitRequest.SendRequest();
    }

    /// <summary>
    /// 退出房間處理
    /// </summary>
    public void ExitRoomResponse()
    {
        uiManager.PopPanel();
    }

    /// <summary>
    /// 按下發送訊息
    /// </summary>
    void OnSendClick()
    {
        gameFace.ButtonClick();

        if (chat_IF.text == "")
        {
            uiManager.ShowTip("發送內容不能為空");
            return;
        }

        chatRequest.SendRequest(chat_IF.text);
        chat_Txt.text += $"<color=#E254DB>自己:{chat_IF.text.Trim()}</color>\n";
        chat_IF.text = "";
        chat_Sb.value = 0;
    }

    /// <summary>
    /// 聊天處理
    /// </summary>
    /// <param name="str">聊天內容</param>
    public void ChatResponse(string str)
    {
        chat_Txt.text += $"<color=#594949>{str}</color>\n";
        chat_Sb.value = 0;
    }

    /// <summary>
    /// 刷新玩家列表
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePlayList(MainPack pack)
    {
        if (pack.RoomPack.Count > 0 && !string.IsNullOrEmpty(pack.RoomPack[0].RoomName))
        {
            roomName_Txt.text = $"房間:{pack.RoomPack[0].RoomName}";
        }        

        //清空
        for (int i = 0; i < userList.childCount; i++)
        {
            Destroy(userList.GetChild(i).gameObject);
        }

        //添加
        int index = 0;
        foreach (PlayerPack player in pack.PlayerPack)
        {
            UserItem userItem = Instantiate(userItemObj, Vector3.zero, Quaternion.identity).GetComponent<UserItem>();
            userItem.transform.SetParent(userList);
            userItem.SetPlayerInfo(player.PlayerName, player.TotalKill, player.SelectCharacter, index == 0, gameFace.UserName == player.PlayerName);

            if (gameFace.UserName == player.PlayerName)
            {
                start_Btn.gameObject.SetActive(index == 0);
                start_Btn.enabled = index == 0;
            }
            index++;
        }
    }

    /// <summary>
    /// 更換角色
    /// </summary>
    /// <param name="index"></param>
    public void ChangeCharacter(int index)
    {
        updateRoomUserInfoRequest.SendRequest(index);
    }

    /// <summary>
    /// 按下開始遊戲
    /// </summary>
    void OnStartClick()
    {
        gameFace.ButtonClick();
        startGameRequest.SendRequest();
    }

    /// <summary>
    /// 開始遊戲處理
    /// </summary>
    /// <param name="pack"></param>
    public void StartGameResponse(MainPack pack)
    {
        switch(pack.ReturnCode)
        {
            case ReturnCode.Fail:
                uiManager.ShowTip("開始遊戲失敗");
                break;
            case ReturnCode.Succeed:
                start_Btn.enabled = false;
                leave_Btn.enabled = false;
                uiManager.ShowTip("準備開始遊戲");
                break;
        }
    }

    /// <summary>
    /// 遊戲開始初始化
    /// </summary>
    /// <param name="pack"></param>
    public void GameStartInit(MainPack pack)
    {
        GamePanel gamePanel = uiManager.PushPanel(PanelType.Game).GetComponent<GamePanel>();
        gamePanel.UpdateGameInfoList(pack);
    }
}
