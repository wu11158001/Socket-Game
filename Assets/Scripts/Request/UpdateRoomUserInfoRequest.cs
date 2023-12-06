using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateRoomUserInfoRequest : BaseRequest
{
    private MainPack pack = null;
    public RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoomUserInfo;
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

    /// <summary>
    /// 發送更換角色協議
    /// </summary>
    /// <param name="pos">選擇角色編號</param>
    public void SendRequest(int characterIndex)
    {
        MainPack pack = new MainPack();
        PlayerPack playerPack = new PlayerPack();

        playerPack.SelectCharacter = characterIndex;

        pack.PlayerPack.Add(playerPack);

        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
