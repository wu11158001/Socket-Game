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
    /// 按下登入
    /// </summary>
    void OnLogonClick()
    {
        //防呆
        if (acc_IF.text == "" || psw_IF.text == "")
        {
            Debug.LogWarning("帳號/密碼不能為空");
            return;
        }

        logonRequest.SendRequest(acc_IF.text, psw_IF.text);
    }

    /// <summary>
    /// 按下切換登入按鈕
    /// </summary>
    void OnSwitchLoginClick()
    {
        uiManager.ClosePanel();
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
                uiManager.PushPanel(PanelType.Login);
                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("註冊失敗!已有相同帳號");
                break;
        }
    }
}
