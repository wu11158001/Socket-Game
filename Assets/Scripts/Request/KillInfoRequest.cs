using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class KillInfoRequest : BaseRequest
{
    private MainPack pack = null;
    public GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.KillInfo;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gamePanel.ShowKillInfo(pack);            
           
            pack = null;
        }
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
