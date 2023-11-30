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

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="pack"></param>
    public void AddPlayer(MainPack pack)
    {
        spawnPos = GameObject.Find("SpawnPos").transform;
        foreach (PlayerPack player in pack.PlayerPack)
        {
            Debug.Log("添加遊戲角色" + player.PlayerName);
            GameObject obj = GameObject.Instantiate(character, spawnPos.position, Quaternion.identity);

            //創建本地角色
            if (player.PlayerName.Equals(gameFace.UserName))
            {
                obj.AddComponent<CharacterController>();
                obj.transform.Find("Weapon").gameObject.AddComponent<WeaponController>();
            }

            //創建其他客戶端角色

            playerDic.Add(player.PlayerName, obj);
        }
    }

    /// <summary>
    /// 客戶端離開遊戲
    /// </summary>
    public void LeaveGame()
    {
        foreach (var c in playerDic.Values)
        {
            GameObject.Destroy(c);
        }
        playerDic.Clear();
    }
}
