using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;
using System.Linq;

public class ServerStartGameRequest : BaseRequest
{
    private MainPack pack = null;
    public RoomPanel roomPanel;

    public override void Awake()
    {
        actionCode = ActionCode.ServerStartGame;
        base.Awake();
    }

    private void Update()
    {
        if(pack != null)
        {
            Debug.Log("遊戲開始");

            gameFace.AddPlayer(pack);
            roomPanel.GameStartInit(pack);

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
