using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class GameExitRequest : BaseRequest
{
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ExitGame;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameFace.LeaveGame();
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
        pack.Str = "GameExit";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
