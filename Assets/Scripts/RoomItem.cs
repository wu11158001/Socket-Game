using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button join_Btn;
    public Text roomName_Txt, count_Txt, state_Txt;
    public RoomListPanel roomListPanel;

    private void Start()
    {
        join_Btn.onClick.AddListener(OnJoinClick);
    }

    /// <summary>
    /// 按下加入房間
    /// </summary>
    void OnJoinClick()
    {
        roomListPanel.JoinRoom(roomName_Txt.text);
    }

    /// <summary>
    /// 設定房間訊息
    /// </summary>
    /// <param name="roomName">房間名</param>
    /// <param name="currCount">當前人數</param>
    /// <param name="maxCount">最大人數</param>
    /// <param name="state">房間狀態</param>
    public void SetRoomInfo(string roomName, int currCount, int maxCount, int state)
    {
        roomName_Txt.text = roomName;
        count_Txt.text = currCount + "/" + maxCount;
        switch (state)
        {
            case 0:
                state_Txt.text = "等待加入";
                break;
            case 1:
                state_Txt.text = "房間已滿";
                break;
            case 2:
                state_Txt.text = "遊戲中";
                break;
        }
    }
}
