using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class ChatRequest : BaseRequest
{
    private string chatStr = null;

    public RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Chat;
        base.Awake();
    }

    private void Update()
    {
        if(chatStr != null)
        {
            roomPanel.ChatResponse(chatStr);
            chatStr = null;
        }
    }

    /// <summary>
    /// 發送協議(發送聊天訊息)
    /// </summary>
    /// <param name="str">訊息內容</param>
    public void SendRequest(string str)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = str;

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        chatStr = pack.Str;
    }
}
