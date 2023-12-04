using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtobuf;

public class LogonPanel : BasePanel
{
    public LogonRequest logonRequest;

    public InputField acc_IF, psw_IF;
    public Button logon_Btn, switchlogin_Btn;

    /// <summary>
    /// UI面板開始
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        Entter();
    }

    /// <summary>
    /// UI面板退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    /// <summary>
    /// UI面板暫停
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    /// <summary>
    /// UI面板繼續
    /// </summary>
    public override void OnRecovery()
    {
        base.OnRecovery();
        Entter();
    }

    /// <summary>
    /// 進入
    /// </summary>
    void Entter()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 離開
    /// </summary>
    void Exit()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        logon_Btn.onClick.AddListener(OnLogonClick);
        switchlogin_Btn.onClick.AddListener(OnSwitchLoginClick);
    }

    /// <summary>
    /// 按下註冊
    /// </summary>
    void OnLogonClick()
    {
        //防呆
        if (acc_IF.text == "" || psw_IF.text == "")
        {
            uiManager.ShowTip("帳號/密碼不能為空");
            return;
        }

        //防呆
        if (acc_IF.text[0] == '0' || psw_IF.text[0] == '0')
        {
            uiManager.ShowTip("帳號/密碼開頭不能為'0'");
            return;
        }

        logonRequest.SendRequest(acc_IF.text, psw_IF.text);
    }

    /// <summary>
    /// 按下切換登入按鈕
    /// </summary>
    void OnSwitchLoginClick()
    {
        uiManager.PopPanel();
    }

    /// <summary>
    /// 協議接收
    /// </summary>
    /// <param name="pack"></param>
    public void OnResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uiManager.ShowTip("註冊成功");
                LoginPanel loginPanel = uiManager.PushPanel(PanelType.Login).GetComponent<LoginPanel>();

                //登入
                loginPanel.OnLogin(acc_IF.text, psw_IF.text);

                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("註冊失敗!已有相同帳號");
                break;
        }
    }
}
