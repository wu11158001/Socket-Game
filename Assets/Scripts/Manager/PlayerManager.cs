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
    /// 添加遊戲玩家
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
                obj.AddComponent<UpdatePosRequest>();
                obj.AddComponent<UpdatePosition>();
                obj.AddComponent<CharacterController>();
                obj.transform.Find("Weapon").gameObject.AddComponent<WeaponController>();
            }
            else
            {
                //創建其他客戶端角色
                obj.GetComponent<Rigidbody2D>().simulated = false;
            }
            
            playerDic.Add(player.PlayerName, obj);
        }
    }

    /// <summary>
    /// 移除遊戲玩家
    /// </summary>
    /// <param name="name"></param>
    public void RemovePlayer(string name)
    {
        if (playerDic.TryGetValue(name, out GameObject obj))
        {
            GameObject.Destroy(obj);
            playerDic.Remove(name);
        }
        else
        {
            Debug.LogError("移除玩家出錯!!!");
        }
    }

    /// <summary>
    /// 更新角色位置
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePos(MainPack pack)
    {
        PosPack posPack = pack.PlayerPack[0].PosPack;
        if (playerDic.TryGetValue(pack.PlayerPack[0].PlayerName, out GameObject obj))
        {
            Vector2 pos = new Vector2(posPack.PosX, posPack.PosY);
            obj.transform.position = pos;
            obj.transform.eulerAngles = new Vector3(0, 0, posPack.CharacterRotZ);

            obj.transform.Find("Weapon").eulerAngles = new Vector3(0, 0, posPack.WeaponRotZ);
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
