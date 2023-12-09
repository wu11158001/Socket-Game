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
    /// <param name="aniHash">動畫名稱</param>
    /// <param name="dir">面相方向(true=右)</param>
    /// <param name="isActive">動畫bool</param>
    public void SendRequest(int aniHash, bool dir, bool isActive)
    {
        MainPack pack = new MainPack();
        AniPack aniPack = new AniPack();
        PlayerPack playerPack = new PlayerPack();

        aniPack.AniHash = aniHash;
        aniPack.IsActive = isActive;
        aniPack.Direction = dir;

        playerPack.PlayerName = gameFace.UserName;
        playerPack.AniPack = aniPack;

        pack.PlayerPack.Add(playerPack);
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = $"{aniHash} = {isActive}";

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
