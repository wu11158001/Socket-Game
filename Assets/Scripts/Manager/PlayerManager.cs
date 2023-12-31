using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace gameFace) : base(gameFace) { }

    //存放房間玩家
    private Dictionary<string, AnimationEvent> playerDic = new Dictionary<string, AnimationEvent>();
    public Dictionary<string, AnimationEvent> GetPlayers { get { return playerDic; } }

    private GameObject[] characters;//角色物件
    private GameObject attackBox;//攻擊框
    private GameObject gem;//本地玩家標記
    private GameObject deshEffect;//翻滾特效

    public override void OnInit()
    {
        base.OnInit();

        characters = Resources.LoadAll<GameObject>("Prefab/Characters");
        attackBox = Resources.Load<GameObject>("Prefab/AttackBox");
        gem = Resources.Load<GameObject>("Prefab/Gem");
        deshEffect = Resources.Load<GameObject>("Prefab/DashEffect");
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

            //翻滾特效
            GameObject deshEff = GameObject.Instantiate(deshEffect);
            deshEff.transform.SetParent(obj.transform);
            deshEff.transform.localPosition = new Vector3(0, -0.7f, 0);
            deshEff.name = "DashEffect";

            SoundRequest soundRequest = obj.AddComponent<SoundRequest>();
            AnimationEvent animationEvent = obj.transform.Find("Body").gameObject.AddComponent<AnimationEvent>();

            //創建本地角色
            if (player.PlayerName.Equals(gameFace.UserName))
            {
                //標記
                GameObject gemObj = GameObject.Instantiate(gem);
                gemObj.transform.SetParent(obj.transform);
                gemObj.transform.localPosition = new Vector3(0, 1.1f, 0);
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
            animationEvent.userName = player.PlayerName;

            playerDic.Add(player.PlayerName, animationEvent);
        }
    }

    /// <summary>
    /// 移除遊戲玩家
    /// </summary>
    /// <param name="name"></param>
    public void RemovePlayer(string name)
    {
        if (playerDic.TryGetValue(name, out AnimationEvent obj))
        {
            GameObject.Destroy(obj.transform.parent.gameObject);
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
        foreach (var player in playerDic.Values)
        {
            GameObject.Destroy(player.transform.parent.gameObject);
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
        if (playerDic.TryGetValue(pack.PlayerPack[0].PlayerName, out AnimationEvent obj))
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
        if (playerDic.TryGetValue(pack.PlayerPack[0].PlayerName, out AnimationEvent obj))
        {
            int aniHash = aniPack.AniHash;
            bool isActive = aniPack.IsActive;
            bool dir = aniPack.Direction;
            obj.UpdateAni(aniHash, dir, isActive);
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
            if (playerDic.TryGetValue(player.PlayerName, out AnimationEvent obj))
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
            if (playerDic.TryGetValue(player.PlayerName, out AnimationEvent obj))
            {
                bool result = pack.ReturnCode == ReturnCode.Succeed ? true : false;
                obj.GameOver(result);
            }
        }
    }
}
