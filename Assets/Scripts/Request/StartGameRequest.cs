using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class StartGameRequest : BaseRequest
{
    public RoomPanel roomPanel;

    private MainPack pack = null;


    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.StartGame;
        base.Awake();
    }

    private void Update()
    {
        if(pack != null)
        {
            roomPanel.StartGameResponse(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送協議
    /// </summary>
    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "StartGame";
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
