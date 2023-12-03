using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace gameFace) : base(gameFace) { }

    //存放玩家
    private Dictionary<string, UpdateCharacterState> playerDic = new Dictionary<string, UpdateCharacterState>();

    private GameObject character;//角色物件
    private GameObject attackBox;//攻擊框

    public override void OnInit()
    {
        base.OnInit();

        character = Resources.Load<GameObject>("Prefab/Character");
        attackBox = Resources.Load<GameObject>("Prefab/AttackBox");
    }

    /// <summary>
    /// 添加遊戲玩家
    /// </summary>
    /// <param name="pack"></param>
    public void AddPlayer(MainPack pack)
    {
        Vector3 spawnPos = Vector3.zero;

        foreach (PlayerPack player in pack.PlayerPack)
        {
            Debug.Log("添加遊戲角色:" + player.PlayerName);
            GameObject obj = GameObject.Instantiate(character, spawnPos, Quaternion.identity);

            Rigidbody2D r2d = obj.AddComponent<Rigidbody2D>();
            r2d.gravityScale = 10;
            r2d.freezeRotation = true;

            UpdateCharacterState body = obj.AddComponent<UpdateCharacterState>();

            //創建本地角色
            if (player.PlayerName.Equals(gameFace.UserName))
            {
                //添加攻擊框
                GameObject aBox = GameObject.Instantiate(attackBox);
                aBox.transform.SetParent(obj.transform);
                aBox.name = "AttackBox";

                obj.AddComponent<UpdatePosRequest>();
                obj.AddComponent<UpdateAinRequest>();
                obj.AddComponent<CharacterController>();
            }
            else
            {
                obj.transform.Find("Gem").gameObject.SetActive(false);
                //創建其他客戶端角色
            }

            obj.name = player.PlayerName;
            body.userName = player.PlayerName;

            playerDic.Add(player.PlayerName, body);
        }
    }

    /// <summary>
    /// 移除遊戲玩家
    /// </summary>
    /// <param name="name"></param>
    public void RemovePlayer(string name)
    {
        if (playerDic.TryGetValue(name, out UpdateCharacterState obj))
        {
            GameObject.Destroy(obj.gameObject);
            playerDic.Remove(name);
        }
        else
        {
            Debug.LogError("移除玩家出錯!!!");
        }
    }

    /// <summary>
    /// 客戶端離開遊戲
    /// </summary>
    public void LeaveGame()
    {
        foreach (var c in playerDic.Values)
        {
            GameObject.Destroy(c.gameObject);
        }
        playerDic.Clear();
    }

    /// <summary>
    /// 更新角色位置
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePos(MainPack pack)
    {
        PosPack posPack = pack.PlayerPack[0].PosPack;
        if (playerDic.TryGetValue(pack.PlayerPack[0].PlayerName, out UpdateCharacterState obj))
        {
            Vector2 pos = new Vector2(posPack.PosX, posPack.PosY);
            obj.UpdatePos(pos);
        }
    }

    /// <summary>
    /// 更新角色動畫
    /// </summary>
    /// <param name="pack"></param>
    public void UpdateAni(MainPack pack)
    {
        AniPack aniPack = pack.PlayerPack[0].AniPack;
        if (playerDic.TryGetValue(pack.PlayerPack[0].PlayerName, out UpdateCharacterState obj))
        {
            string aniName = aniPack.AnimationName;
            bool isActive = aniPack.IsActive;
            bool dir = aniPack.Direction;
            obj.UpdateAni(aniName, dir, isActive);
        }
    }

    /// <summary>
    /// 玩家攻擊
    /// </summary>
    /// <param name=""></param>
    public void PlayerAttack(MainPack pack)
    {
        foreach (var player in pack.PlayerPack)
        {
            if (playerDic.TryGetValue(player.PlayerName, out UpdateCharacterState obj))
            {
                obj.Hurt();
            }
        }
    }
}
