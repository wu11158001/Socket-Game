using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class PlayersRequest : BaseRequest
{
    private MainPack pack;
    public RoomPanel roomPanel;

    public override void Awake()
    {
        actionCode = ActionCode.PlayerList;
        base.Awake();
    }

    private void Update()
    {
        if(pack != null)
        {
            roomPanel.UpdatePlayList(pack);
            pack = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
