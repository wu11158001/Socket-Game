using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateCharacterListRequest : BaseRequest
{
    private MainPack pack = null;

    public GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateCharacterList;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gamePanel.UpdateGameInfoList(pack);
            gameFace.RemovePlayer(pack.Str);
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
