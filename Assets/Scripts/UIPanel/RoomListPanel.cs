using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtobuf;

public class RoomListPanel : BasePanel
{
    public Button logout_Btn, serch_Btn, create_Btn, joinRoom_Btn;
    public InputField createName_IF, joinRoom_IF;
    public Slider count_Sli;
    public Text count_Txt;

    public Transform roomListTransform;
    public GameObject roomIten;

    public LogoutRequest logoutRequest;
    public CreateRoomRequest createRoomRequest;
    public SearchRoomRequest serchRoomRequest;
    public JoinRoomRequest joinRoomRequest;

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
        //停止更新房間
        CancelInvoke(nameof(OnSearchClick));

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
        //定時更新房間
        InvokeRepeating(nameof(OnSearchClick), 0.3f, 5);

        joinRoom_IF.text = "";
        createName_IF.text = "";
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
        logout_Btn.onClick.AddListener(OnLogoutClick);
        serch_Btn.onClick.AddListener(OnSearchClick);
        create_Btn.onClick.AddListener(OnCreateClick);
        joinRoom_Btn.onClick.AddListener(delegate { JoinRoom(joinRoom_IF.text); });
        count_Sli.onValueChanged.AddListener((val => { count_Txt.text = $"{val}人"; }));
    }

    /// <summary>
    /// 退出登入
    /// </summary>
    void OnLogoutClick()
    {
        logoutRequest.SendRequest();
        uiManager.PopPanel();        
    }

    /// <summary>
    /// 查詢房間
    /// </summary>
    void OnSearchClick()
    {
        serchRoomRequest.SendRequest();
    }

    /// <summary>
    /// 創建房間
    /// </summary>
    void OnCreateClick()
    {
        if(createName_IF.text == "") createName_IF.text = gameFace.UserName;
        createRoomRequest.SendRequest(createName_IF.text, (int)count_Sli.value);
    }

    /// <summary>
    /// 創建房間協議接收
    /// </summary>
    /// <param name="pack"></param>
    public void CreateRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                //uiManager.ShowTip("創建成功");
                RoomPanel roomPanel = uiManager.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayList(pack);
                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("創建失敗");
                break;
        }
    }

    /// <summary>
    /// 搜尋房間協議接收
    /// </summary>
    /// <param name="pack"></param>
    public void SearchRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                //uiManager.ShowTip("刷新房間。共 " + pack.RoomPack.Count + " 間房間");
                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("查詢出錯");
                break;
            case ReturnCode.NotRoom:
                //uiManager.ShowTip("目前沒有房間");
                break;
        }

        UpdateRoomList(pack);
    }

    /// <summary>
    /// 更新房間列表
    /// </summary>
    /// <param name="pack"></param>
    void UpdateRoomList(MainPack pack)
    {
        //清空房間列表
        for (int i = 0; i < roomListTransform.childCount; i++)
        {
            Destroy(roomListTransform.GetChild(i).gameObject);
        }

        //添加房間
        foreach (RoomPack room in pack.RoomPack)
        {
            RoomItem item = Instantiate(roomIten, Vector3.zero, Quaternion.identity).GetComponent<RoomItem>();
            item.roomListPanel = this;
            item.transform.SetParent(roomListTransform);
            item.SetRoomInfo(room.CurrCount, room.MaxCount, room.State, room.RoomName);
        }
    }

    /// <summary>
    /// 加入房間
    /// </summary>
    /// <param name="roomName">房間名</param>
    public void JoinRoom(string roomName)
    {
        joinRoomRequest.SendRequest(roomName);
    }

    /// <summary>
    /// 加入房間回復
    /// </summary>
    /// <param name="pack"></param>
    public void JoinRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                RoomPanel roomPanel = uiManager.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayList(pack);
                //uiManager.ShowTip("加入房間成功");
                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("加入房間失敗");
                break;
            default:
                uiManager.ShowTip("房間不存在");
                break;
        }
    }
}
