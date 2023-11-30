using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class GameExitRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Exit;
        base.Awake();
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
}
