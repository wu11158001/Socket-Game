using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button join_Btn;
    public Text roomName_Txt, count_Txt, state_Txt;

    [HideInInspector] 
    public RoomListPanel roomListPanel;

    private string roomName;

    private void Start()
    {
        transform.localScale = Vector3.one;
        join_Btn.onClick.AddListener(OnJoinClick);
    }

    /// <summary>
    /// 按下加入房間
    /// </summary>
    void OnJoinClick()
    {
        roomListPanel.JoinRoom(roomName);
    }

    /// <summary>
    /// 設定房間訊息
    /// </summary>
    /// <param name="currCount">當前人數</param>
    /// <param name="maxCount">最大人數</param>
    /// <param name="state">房間狀態</param>
    /// <param name="roomName">房間名</param>
    public void SetRoomInfo(int currCount, int maxCount, int state, string roomName)
    {
        this.roomName = roomName;

        roomName_Txt.text = $"房間:{roomName}";
        count_Txt.text = currCount + " / " + maxCount;
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
