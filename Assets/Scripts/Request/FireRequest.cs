using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class FireRequest : BaseRequest
{
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Fire;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameFace.SpawnBullet(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 發送協議
    /// </summary>
    /// <param name="pos">子彈位置</param>
    /// <param name="rot">子彈旋轉</param>
    public void SendRequest(Vector2 pos, float rot, Vector2 mousePos)
    {
        MainPack pack = new MainPack();
        BulletPack bulletPack = new BulletPack();

        bulletPack.PosX = pos.x;
        bulletPack.PosY = pos.y;
        bulletPack.RotZ = rot;
        bulletPack.MousePosX = mousePos.x;
        bulletPack.MousePosY = mousePos.y;

        pack.BulletPack = bulletPack;
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequestUDP(pack);
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
