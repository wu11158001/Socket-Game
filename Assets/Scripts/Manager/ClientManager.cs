using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using SocketGameProtobuf;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;

    public ClientManager(GameFace gameFace) : base(gameFace) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
    }

    /// <summary>
    /// 初始化連接
    /// </summary>
    void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("127.0.0.1", 6666);
            //開始接收訊息
            StartReceive();
            gameFace.ShowTip("連接成功");
        }
        catch (Exception e)
        {
            Debug.LogWarning("連接失敗:" + e);
            gameFace.ShowTip("連接失敗");
        }
    }

    /// <summary>
    /// 關閉連接
    /// </summary>
    void CloseSocket()
    {
        if(socket.Connected && socket != null)
        {
            socket.Close();
        }
    }

    /// <summary>
    /// 開始接收訊息
    /// </summary>
    void StartReceive()
    {
        socket.BeginReceive(message.GetBuffer, message.GetStartIndex, message.GetRemSize, SocketFlags.None, ReceiveCallBack, null);
    }
    /// <summary>
    /// 接收訊息CallBack
    /// </summary>
    /// <param name="iar"></param>
    void ReceiveCallBack(IAsyncResult iar)
    {
        try
        {
            if (socket == null || !socket.Connected) return;

            int len = socket.EndReceive(iar);
            if(len == 0)
            {
                //關閉連接
                CloseSocket();
                return;
            }

            message.ReadBuffer(len, HandleResponse);
            //重新開始接收訊息
            StartReceive();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 處理回覆
    /// </summary>
    /// <param name="pack"></param>
    void HandleResponse(MainPack pack)
    {
        gameFace.HandleResponse(pack);
    }

    /// <summary>
    /// 發送訊息
    /// </summary>
    /// <param name="pack"></param>
    public void Send(MainPack pack)
    {
        socket.Send(Message.PackData(pack));
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;

        //關閉連接
        CloseSocket();
    }
}
