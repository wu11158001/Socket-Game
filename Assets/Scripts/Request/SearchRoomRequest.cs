using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class SearchRoomRequest : BaseRequest
{
    private MainPack pack = null;
    public RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.SearchRoom;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            roomListPanel.SearchRoomResponse(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送請求
    /// </summary>
    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "SreachRoom";
        base.SendRequest(pack);
    }

    /// <summary>
    /// 接收協議
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
