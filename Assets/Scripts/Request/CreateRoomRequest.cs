using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class CreateRoomRequest : BaseRequest
{
    private MainPack pack = null;
    public RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    private void Update()
    {
        if(pack != null)
        {
            roomListPanel.CreateRoomResponse(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送請求
    /// </summary>
    /// <param name="roomName">房間名</param>
    /// <param name="maxCount">最大人數</param>
    public void SendRequest(string roomName, int maxCount)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        RoomPack room = new RoomPack();
        room.RoomName = roomName;
        room.MaxCount = maxCount;
        pack.RoomPack.Add(room);

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
