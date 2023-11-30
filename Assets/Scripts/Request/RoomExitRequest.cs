using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class RoomExitRequest : BaseRequest
{
    public RoomPanel roomPanel;

    private bool isExit = false;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ExitGame;
        base.Awake();
    }

    private void Update()
    {
        if(isExit)
        {
            roomPanel.ExitRoomResponse();
            isExit = false;
        }
    }

    /// <summary>
    /// 發送離開房間
    /// </summary>
    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "RoomExit";

        base.SendRequest(pack);
    }

    /// <summary>
    /// 接收協議
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        isExit = true;
    }
}
