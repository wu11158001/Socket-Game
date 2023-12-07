using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtobuf;

public class LoginPanel : BasePanel
{
    public LoginRequest loginRequest;

    public InputField acc_IF, psw_IF;
    public Button login_Btn, switchLogin_Btn;

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
        login_Btn.onClick.AddListener(OnLogonClick);
        switchLogin_Btn.onClick.AddListener(OnSwitchLogonClick);

        //自動登入
        string acc = PlayerPrefs.GetString("StockGame_Acc");
        string psw = PlayerPrefs.GetString("StockGame_Psw");
        if (!string.IsNullOrEmpty(acc) && !string.IsNullOrEmpty(psw))
        {
            acc_IF.text = acc;
            psw_IF.text = psw;
            OnLogin(acc, psw);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLogonClick();
        }

        if (acc_IF.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            psw_IF.ActivateInputField();
        }
    }

    /// <summary>
    /// 按下登入
    /// </summary>
    void OnLogonClick()
    {
        gameFace.ButtonClick();

        //防呆
        if (acc_IF.text == "" || psw_IF.text == "")
        {
            Debug.LogWarning("帳號/密碼不能為空");
            return;
        }

        OnLogin(acc_IF.text, psw_IF.text);
    }

    /// <summary>
    /// 登入
    /// </summary>
    /// <param name="acc"></param>
    /// <param name="psw"></param>
    public void OnLogin(string acc, string psw)
    {
        //本地紀錄
        PlayerPrefs.SetString("StockGame_Acc", acc);
        PlayerPrefs.SetString("StockGame_Psw", psw);

        loginRequest.SendRequest(acc, psw);
    }


    /// <summary>
    /// 按下切換註冊按鈕
    /// </summary>
    void OnSwitchLogonClick()
    {
        gameFace.ButtonClick();
        uiManager.PushPanel(PanelType.Logon);
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
                gameFace.UserName = acc_IF.text;
                uiManager.PushPanel(PanelType.RoomList);
                break;
            case ReturnCode.Fail:
                uiManager.ShowTip("登入失敗");
                break;
            case ReturnCode.Duplicate:
                uiManager.ShowTip("帳號已登入");
                break;
        }
    }
}
