using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class GameResultRequest : BaseRequest
{
    private MainPack pack = null;

    public GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameResult;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gamePanel.SetGameOverInfo(pack);
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
