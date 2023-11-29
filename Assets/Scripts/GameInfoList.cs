using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoList : MonoBehaviour
{
    [SerializeField]
    private Text userName_Txt;
    [SerializeField]
    private Slider hp_Sli;

    /// <summary>
    /// 設定訊息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    public void SetInfo(string name, int hp)
    {
        userName_Txt.text = name;
        hp_Sli.value = hp;
    }

    /// <summary>
    /// 更新遊戲列表內容
    /// </summary>
    /// <param name="hp"></param>
    public void UpdateValue(int hp)
    {
        hp_Sli.value = hp;
    }
}
