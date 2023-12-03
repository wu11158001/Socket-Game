using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using Google.Protobuf.Collections;

public class AttackRequest : BaseRequest
{
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.PlayerAttack;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameFace.PlayerAttack(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送攻擊協議
    /// </summary>
    /// <param name="hitList">被攻擊玩家</param>
    public void SendRequest(List<string> hitList)
    {
        MainPack pack = new MainPack();
        AttackPack attackPack = new AttackPack();
        PlayerPack playerPack = new PlayerPack();

        attackPack.AttackNames.Add(hitList);

        playerPack.PlayerName = gameFace.UserName;
        playerPack.AttackPack = attackPack;

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
