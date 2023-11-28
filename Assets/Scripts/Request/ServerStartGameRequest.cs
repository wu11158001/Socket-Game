using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class ServerStartGameRequest : BaseRequest
{
    private bool isStartGame = false;

    public override void Awake()
    {
        actionCode = ActionCode.ServerStartGame;
        base.Awake();
    }

    private void Update()
    {
        if(isStartGame)
        {
            Debug.Log("遊戲開始");
            isStartGame = false;
        }
    }

    /// <summary>
    /// 接收協議
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        isStartGame = true;
    }
}
