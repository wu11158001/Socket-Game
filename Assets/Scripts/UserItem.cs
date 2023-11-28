using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    [SerializeField]
    private Text playerName;

    /// <summary>
    /// 設定玩家訊息
    /// </summary>
    /// <param name="playerName">玩家名稱</param>
    public void SetPlayerInfo(string playerName)
    {
        this.playerName.text = playerName;
    }
}
