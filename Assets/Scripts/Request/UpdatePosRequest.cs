using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdatePosRequest : BaseRequest
{
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdatePos;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameFace.UpdatePos(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送協議
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="characterRotZ">角色旋轉</param>
    /// <param name="gunRotZ">武器旋轉</param>
    public void SendRequest(Vector2 pos, float characterRotZ, float gunRotZ)
    {
        MainPack pack = new MainPack();
        PosPack posPack = new PosPack();
        PlayerPack playerPack = new PlayerPack();

        posPack.PosX = pos.x;
        posPack.PosY = pos.y;
        posPack.CharacterRotZ = characterRotZ;
        posPack.WeaponRotZ = gunRotZ;

        playerPack.PlayerName = gameFace.UserName;
        playerPack.PosPack = posPack;

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
