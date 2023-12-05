using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    [SerializeField]
    private Text playerName_Txt, killCount_Txt;
    [SerializeField]
    private Image RoomMaster_Img;

    /// <summary>
    /// 設定玩家訊息
    /// </summary>
    /// <param name="userName">玩家名稱</param>
    /// <param name="kills">擊殺數</param>
    /// <param name="isMaster">房主</param>
    /// <param name="isSelf">本地玩家</param>
    public void SetPlayerInfo(string userName, int kills, bool isMaster, bool isSelf)
    {
        transform.localScale = Vector3.one;

        playerName_Txt.text = userName;
        killCount_Txt.text = $"擊殺數:{kills}";

        string colorStr = isSelf ? "#3EE510" : "#CFBBBB";
        if (ColorUtility.TryParseHtmlString(colorStr, out Color textColor))
        {
            playerName_Txt.color = textColor;
        }

        RoomMaster_Img.enabled = isMaster;

    }
}
