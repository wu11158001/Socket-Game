using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadInfo : MonoBehaviour
{
    [SerializeField] private Image healthBar_Img;
    [SerializeField] private Text playerName_Txt;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 設定初始訊息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="nameColor"></param>
    /// <param name="hp"></param>
    public void SetInitInfo(string name, string nameColor, float hp)
    {
        playerName_Txt.text = name;
        if (ColorUtility.TryParseHtmlString(nameColor, out Color textColor))
        {
            playerName_Txt.color = textColor;
        }
        healthBar_Img.fillAmount = hp;
    }

    /// <summary>
    /// 設定位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetPos(Vector2 pos)
    {
        rectTransform.anchoredPosition = pos;
    }

    /// <summary>
    /// 設定血量值
    /// </summary>
    /// <param name="value"></param>
    public void SetHPValue(float value)
    {
        healthBar_Img.fillAmount = value;
    }
}
