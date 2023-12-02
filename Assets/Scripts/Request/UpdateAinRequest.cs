using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateAinRequest : BaseRequest
{
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateAni;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameFace.UpdateAni(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送動畫協議
    /// </summary>
    /// <param name="aniName">動畫名稱</param>
    /// <param name="dir">面相方向(true=左)</param>
    /// <param name="isActive">動畫bool</param>
    public void SendRequest(string aniName, bool dir, bool isActive = true)
    {
        MainPack pack = new MainPack();
        StatePack statePack = new StatePack();
        PlayerPack playerPack = new PlayerPack();

        statePack.AnimationName = aniName;
        statePack.IsActive = isActive;
        statePack.Direction = dir;

        playerPack.PlayerName = gameFace.UserName;
        playerPack.StatePack = statePack;

        pack.PlayerPack.Add(playerPack);
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

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
