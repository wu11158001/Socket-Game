using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace gameFace) : base(gameFace) { }

    //存放玩家
    private Dictionary<string, GameObject> playerDic = new Dictionary<string, GameObject>();

    private GameObject character;//角色物件
    private Transform spawnPos;//出生點

    public override void OnInit()
    {
        base.OnInit();

        character = Resources.Load<GameObject>("Prefab/Character");
    }

    //玩家當前ID
    public string curPlayerID { get; set; }

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="pack"></param>
    public void AddPlayer(List<PlayerPack> pack)
    {
        spawnPos = GameObject.Find("SpqwnPos").transform;
        foreach (PlayerPack player in pack)
        {
            GameObject obj = GameObject.Instantiate(character, spawnPos.position, Quaternion.identity);

            //創建本地角色
            if (player.PlayerName.Equals(curPlayerID))
            {
                
            }

            //創建其他客戶端角色

            playerDic.Add(player.PlayerName, obj);
        }
    }

    /// <summary>
    /// 客戶端離開遊戲
    /// </summary>
    /// <param name="id"></param>
    public void LeaveGame(string id)
    {
        if (playerDic.TryGetValue(id, out GameObject obj))
        {
            GameObject.Destroy(obj);
            playerDic.Remove(id);
        }
        else
        {
            Debug.LogError("移除角色出錯");
        }
    }
}
