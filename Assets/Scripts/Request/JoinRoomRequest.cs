using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class JoinRoomRequest : BaseRequest
{
    private MainPack pack = null;
    public RoomListPanel roomListPanel;
    
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            roomListPanel.JoinRoomResponse(pack);
            pack = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }

    /// <summary>
    /// 發送協議
    /// </summary>
    /// <param name="roomName">房間名</param>
    public void SendRequest(string roomName)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = roomName;

        base.SendRequest(pack);
    }
}
