using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace gameFace) : base(gameFace) { }

    //存放房間玩家
    private Dictionary<string, UpdateCharacterState> playerDic = new Dictionary<string, UpdateCharacterState>();
    public Dictionary<string, UpdateCharacterState> GetPlayers { get { return playerDic; } }

    private GameObject[] characters;//角色物件
    private GameObject attackBox;//攻擊框
    private GameObject gem;//本地玩家標記

    public override void OnInit()
    {
        base.OnInit();

        characters = Resources.LoadAll<GameObject>("Prefab/Characters");
        attackBox = Resources.Load<GameObject>("Prefab/AttackBox");
        gem = Resources.Load<GameObject>("Prefab/Gem");
    }

    /// <summary>
    /// 添加遊戲玩家
    /// </summary>
    /// <param name="pack"></param>
    public void AddPlayer(MainPack pack)
    {
        //產生位置
        float posX = Random.Range(-10, 10);
        Vector3 spawnPos = new Vector3(posX, -3, 0);

        foreach (PlayerPack player in pack.PlayerPack)
        {
            Debug.Log("添加遊戲角色:" + player.PlayerName);
            GameObject obj = GameObject.Instantiate(characters[player.SelectCharacter], spawnPos, Quaternion.identity);

            Rigidbody2D r2d = obj.AddComponent<Rigidbody2D>();
            r2d.gravityScale = 10;
            r2d.freezeRotation = true;

            UpdateCharacterState body = obj.AddComponent<UpdateCharacterState>();

            //創建本地角色
            if (player.PlayerName.Equals(gameFace.UserName))
            {
                //標記
                GameObject gemObj = GameObject.Instantiate(gem);
                gemObj.transform.SetParent(obj.transform);
                gemObj.transform.localPosition = new Vector3(0, 0.78f, 0);
                gemObj.name = "Gem";

                //添加攻擊框
                GameObject attackBoxObj = GameObject.Instantiate(attackBox);
                attackBoxObj.transform.SetParent(obj.transform);
                attackBoxObj.transform.localPosition = Vector3.zero;
                attackBoxObj.name = "AttackBox";

                obj.AddComponent<UpdatePosRequest>();
                obj.AddComponent<UpdateAinRequest>();
                obj.AddComponent<UserController>();
            }
            else
            {
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

    /// <summary>
    /// 遊戲結果
    /// </summary>
    /// <param name=""></param>
    public void GameResult(MainPack pack)
    {
        foreach (var player in pack.PlayerPack)
        {
            if (playerDic.TryGetValue(player.PlayerName, out UpdateCharacterState obj))
            {
                bool result = pack.ReturnCode == ReturnCode.Succeed ? true : false;
                obj.GameOver(result);
            }
        }
    }
}
