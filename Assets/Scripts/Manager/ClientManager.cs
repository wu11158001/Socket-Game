using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using SocketGameProtobuf;
using System.Threading;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;
    private const string ip = "127.0.0.1";

    public ClientManager(GameFace gameFace) : base(gameFace) { }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
        InitUDP();
    }

    /// <summary>
    /// 初始化連接
    /// </summary>
    void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(ip, 6666);
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

    #region UDP協議

    private Socket udpClient;
    private IPEndPoint ipEndPoint;
    private EndPoint endPoint;
    private Byte[] buffer = new Byte[1024];
    private Thread aucThread;

    /// <summary>
    /// 初始化UDP
    /// </summary>
    void InitUDP()
    {
        udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 6667);
        endPoint = ipEndPoint;

        try
        {
            udpClient.Connect(endPoint);
        }
        catch (Exception)
        {
            Debug.LogError("UDP連接失敗!!!");
            return;
        }
        aucThread = new Thread(ReceiceMsg);
        aucThread.Start();
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    private void ReceiceMsg()
    {
        Debug.Log("UDP開始接收");
        while (true)
        {
            int len = udpClient.ReceiveFrom(buffer, ref endPoint);
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
            Debug.Log("UDP接收數據:" + pack.User + " => " + pack.ActionCode.ToString());
            HandleResponse(pack);
        }
    }

    /// <summary>
    /// 發送請求UDP
    /// </summary>
    /// <param name="pack"></param>
    public void SendUDP(MainPack pack)
    {
        Byte[] sendBuff = Message.PackDataUDP(pack);
        udpClient.Send(sendBuff, sendBuff.Length, SocketFlags.None);
    }

    #endregion

    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;

        //關閉連接
        CloseSocket();
    }
}
