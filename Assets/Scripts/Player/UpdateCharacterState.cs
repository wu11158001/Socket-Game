using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateCharacterState : MonoBehaviour
{
    private UserController userController;
    private AnimationEvent animationEvent;

    private SpriteRenderer body;
    private Animator animator;

    public string userName;

    private float limitPosX = 17;//移動限制範圍

    private bool isActionable = true;//角色是否可行動
    public bool GetActionable { get { return isActionable; } }

    private void Start()
    {
        userController = GetComponent<UserController>();
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();
        body = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //移動
        if (!animator.GetBool(AnimatorHash.IsDash) && !animator.GetBool(AnimatorHash.IsAttack))
        {
            if (animator.GetBool(AnimatorHash.IsRun))
            {
                int dir = body.flipX ? -1 : 1;
                transform.position = new Vector3(transform.position.x + 10 * dir * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }

        //限制移動範圍
        if (transform.position.x > limitPosX) transform.position = new Vector2(limitPosX, transform.position.y); 
        else if(transform.position.x < -limitPosX) transform.position = new Vector2(-limitPosX, transform.position.y);
    }

    /// <summary>
    /// 刷新位置
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="dir">面相方向(true=左)</param>
    public void UpdatePos(Vector2 pos)
    {
        transform.position = pos;
    }

    /// <summary>
    /// 更新動畫
    /// </summary>
    /// <param name="aniName">動畫名稱</param>
    /// <param name="dir">面相方向(true=右)</param>
    /// <param name="isActive">動畫bool</param>
    public void UpdateAni(string aniName, bool dir, bool isActive)
    {
        body.flipX = dir;

        if (aniName == "Hurt_Tr" || aniName == "Die_Tr" || aniName == "Win_Tr")
        {
            animator.SetTrigger(aniName);
            return;
        }
        
        animator.SetBool(aniName, isActive);

        if (aniName == "IsDash" && isActive == false) animationEvent.StopDash();
    }

    /// <summary>
    /// 受傷
    /// </summary>
    public void Hurt()
    {
        animationEvent.Hurt();
    }

    /// <summary>
    /// 遊戲結果
    /// </summary>
    /// <param name="result">是否獲勝</param>
    public void GameOver(bool result)
    {
        isActionable = false;

        string triggerName = result ? "Win_Tr" : "Die_Tr";
        if (userController) userController.GameOver(triggerName);
        animator.SetTrigger(triggerName);
    }
}
