using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class LoginRequest : BaseRequest
{
    public LoginPanel loginPanel;
    private MainPack pack;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        base.Awake();
    }

    private void Update()
    {
        if(pack != null)
        {
            loginPanel.OnResponse(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 協議接收
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }

    /// <summary>
    /// 發送請求
    /// </summary>
    /// <param name="acc">帳號</param>
    /// <param name="psw">密碼</param>
    public void SendRequest(string acc, string psw)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        LoginPack loginPack = new LoginPack();
        loginPack.UserName = acc;
        loginPack.Password = psw;
        pack.LoginPack = loginPack;

        base.SendRequest(pack);
    }
}
