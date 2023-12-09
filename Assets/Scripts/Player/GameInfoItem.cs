using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoItem : MonoBehaviour
{
    [SerializeField]
    private Text userName_Txt, totalKill_Txt;
    [SerializeField]
    private Slider hp_Sli;

    private string killStr = "慘忍度";
    private int killCount;

    /// <summary>
    /// 設定訊息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    /// <param name="kills"></param>
    public void SetInfo(string name, int hp, int kills)
    {
        userName_Txt.text = name;
        totalKill_Txt.text = $"{killStr}:{kills}";
        hp_Sli.value = hp;

        killCount = kills;
    }

    /// <summary>
    /// 擊殺數增加
    /// </summary>
    public void AddKillCount()
    {
        killCount++;
        totalKill_Txt.text = $"{killStr}:{killCount}";
    }
}
