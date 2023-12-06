using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    [SerializeField]
    private Text playerName_Txt, killCount_Txt;
    [SerializeField]
    private Image roomMaster_Img, avatar_Img;
    [SerializeField]
    private Button selectLeft_Btn, selectRight_Btn;
    [SerializeField]
    private Sprite[] selectAvatar;

    private int curIndex = 0;
    private RoomPanel roomPanel;

    private void Start()
    {
        roomPanel = GameObject.FindObjectOfType<RoomPanel>();

        selectLeft_Btn.onClick.AddListener(delegate { SelectCharacter(-1); });
        selectRight_Btn.onClick.AddListener(delegate { SelectCharacter(1); });
    }

    /// <summary>
    /// 選擇角色
    /// </summary>
    /// <param name="dir"></param>
    void SelectCharacter(int dir)
    {
        if (curIndex + dir > selectAvatar.Length - 1) curIndex = 0;
        else if (curIndex + dir < 0) curIndex = selectAvatar.Length - 1;
        else curIndex += dir;
        avatar_Img.sprite = selectAvatar[curIndex];

        roomPanel.ChangeCharacter(curIndex);
    }

    /// <summary>
    /// 設定玩家訊息
    /// </summary>
    /// <param name="userName">玩家名稱</param>
    /// <param name="kills">擊殺數</param>
    /// <param name="characterIndex">角色編號</param>
    /// <param name="isMaster">房主</param>
    /// <param name="isSelf">本地玩家</param>
    public void SetPlayerInfo(string userName, int kills, int characterIndex, bool isMaster, bool isSelf)
    {
        transform.localScale = Vector3.one;

        playerName_Txt.text = userName;
        killCount_Txt.text = $"擊殺數:{kills}";

        string colorStr = isSelf ? "#3EE510" : "#CFBBBB";
        if (ColorUtility.TryParseHtmlString(colorStr, out Color textColor))
        {
            playerName_Txt.color = textColor;
        }

        avatar_Img.sprite = selectAvatar[characterIndex];

        selectLeft_Btn.gameObject.SetActive(isSelf);
        selectRight_Btn.gameObject.SetActive(isSelf);

        roomMaster_Img.enabled = isMaster;
    }
}
