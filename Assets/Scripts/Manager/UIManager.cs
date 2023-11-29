using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    public UIManager(GameFace gameFace) : base(gameFace) { }

    private Transform canvasTransform;

    //紀錄已生成的UI
    private Dictionary<PanelType, BasePanel> panelDic = new Dictionary<PanelType, BasePanel>();
    //紀錄UI資源路徑
    private Dictionary<PanelType, string> panelPathDic = new Dictionary<PanelType, string>();
    //場景顯示的UI
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    //提示框面板
    private TipPanel tipPanel;

    public override void OnInit()
    {
        base.OnInit();

        canvasTransform = GameObject.Find("Canvas").transform;

        //初始化UI路徑
        IninPanel();

        PushPanel(PanelType.Tip);
        PushPanel(PanelType.Start);        
    }

    /// <summary>
    /// 初始化UI路徑
    /// </summary>
    void IninPanel()
    {
        string panelPath = "Panel/";
        string[] panelName = new string[]
        {
            "StartPanel",//開始面板
            "LoginPanel",//登入面板
            "LogonPanel",//註冊面板
            "TipPanel",//提示面板
            "RoomListPanel",//房間列表面板
            "RoomPanel",//房間面板
            "GamePanel",//遊戲面板
        };

        panelPathDic.Add(PanelType.Start, panelPath + panelName[0]);//開始面板
        panelPathDic.Add(PanelType.Login, panelPath + panelName[1]);//登入面板
        panelPathDic.Add(PanelType.Logon, panelPath + panelName[2]);//註冊面板
        panelPathDic.Add(PanelType.Tip, panelPath + panelName[3]);//提示面板
        panelPathDic.Add(PanelType.RoomList, panelPath + panelName[4]);//房間列表面板
        panelPathDic.Add(PanelType.Room, panelPath + panelName[5]);//房間面板
        panelPathDic.Add(PanelType.Game, panelPath + panelName[6]);//遊戲面板
    }

    /// <summary>
    /// 實例化UI
    /// </summary>
    /// <param name="panelType"></param>
    BasePanel SpawnPanel(PanelType panelType)
    {
        if(panelPathDic.TryGetValue(panelType, out string path))
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
            obj.transform.SetParent(canvasTransform, false);
            BasePanel basePanel = obj.GetComponent<BasePanel>();
            basePanel.SetUIManager = this;
            panelDic.Add(panelType, basePanel);
            return basePanel;
        }

        return null;
    }

    /// <summary>
    /// 顯示UI
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(PanelType panelType)
    {
        if(panelDic.TryGetValue(panelType, out BasePanel panel))
        {
            if(panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            panelStack.Push(panel);
            panel.OnEnter();
            return panel;
        }
        else
        {

            //實例化UI
            BasePanel newPanel = SpawnPanel(panelType);

            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            panelStack.Push(newPanel);
            newPanel.OnEnter();
            return newPanel;
        }
    }

    /// <summary>
    /// 關閉當前UI面板
    /// </summary>
    public void ClosePanel()
    {
        if (panelStack.Count == 0) return;

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        BasePanel nextPanel = panelStack.Peek();
        nextPanel.OnRecovery();
    }

    /// <summary>
    /// 設定提示框面板
    /// </summary>
    /// <param name="panel"></param>
    public void SetTipPanel(TipPanel panel)
    {
        tipPanel = panel;
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    /// <param name="isSync">是否為異步</param>
    public void ShowTip(string str, bool isSync = false)
    {
        tipPanel.ShowTip(str, isSync);
    }
}
