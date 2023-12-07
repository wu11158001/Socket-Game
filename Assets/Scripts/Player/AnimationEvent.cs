using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public SoundRequest soundRequest;

    private UserController userController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private AnimationEvent animationEvent;
    private Rigidbody2D r2d;
    private BoxCollider2D box2d;

    private float initGravity;
    public string userName;

    private float limitPosX = 17;//移動限制範圍

    private bool isActionable = true;//角色是否可行動
    public bool GetActionable { get { return isActionable; } }

    void Start()
    {
        animator = GetComponent<Animator>();
        userController = GetComponent<UserController>();
        animationEvent = GetComponent<AnimationEvent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        r2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();

        initGravity = r2d.gravityScale;
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //移動
        if (!animator.GetBool(AnimatorHash.IsDash) && !animator.GetBool(AnimatorHash.IsAttack))
        {
            if (animator.GetBool(AnimatorHash.IsRun))
            {
                int dir = spriteRenderer.flipX ? -1 : 1;
                transform.position = new Vector3(transform.position.x + 10 * dir * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }

        //動畫完畢執行
        if (stateInfo.normalizedTime >= 1f)
        {
            if (animator.GetBool(AnimatorHash.IsAttack)) animationEvent.StopAttack();
            if (animator.GetBool(AnimatorHash.IsJumpAttack)) animationEvent.StopJumpAttack();
        }

        //限制移動範圍
        if (transform.position.x > limitPosX) transform.position = new Vector2(limitPosX, transform.position.y);
        else if (transform.position.x < -limitPosX) transform.position = new Vector2(-limitPosX, transform.position.y);
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
        spriteRenderer.flipX = dir;

        if (aniName == "Hurt_Tr" || aniName == "Die_Tr" || aniName == "Win_Tr")
        {
            animator.SetTrigger(aniName);
            return;
        }

        animator.SetBool(aniName, isActive);

        if (aniName == "IsDash" && isActive == false) animationEvent.StopDash();
    }

    /// <summary>
    /// 播放待機
    /// </summary>
    public void PlayIdle()
    {
        animator.Play(AnimatorHash.IsIdle);
        if (userController) userController.HurtOver();
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    void OnJump()
    {
        r2d.simulated = true;
        r2d.velocity = new Vector2(0, 25);
    }

    /// <summary>
    /// 翻滾(閃躲)
    /// </summary>
    void OnDash()
    {
        soundRequest.PlaySound("Dash");

        r2d.gravityScale = 0;
        box2d.enabled = false;

        int forceDir = spriteRenderer.flipX ? -1 : 1;
        r2d.velocity = new Vector2(13 * forceDir, 0);
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    public void StopDash()
    {
        animator.SetBool(AnimatorHash.IsDash, false);

        r2d.gravityScale = initGravity;
        box2d.enabled = true;

        if (userController) userController.StopDash();
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    void OnAttack()
    {
        soundRequest.PlaySound("Attack");

        int forceDir = spriteRenderer.flipX ? -1 : 1;
        r2d.velocity = new Vector2(5 * forceDir, 0);

        if (userController) userController.OpenAttackBox();
    }

    /// <summary>
    /// 停止攻擊
    /// </summary>
    public void StopAttack()
    {
        animator.SetBool(AnimatorHash.IsAttack, false);
        if (userController) userController.StopAttack();
    }

    /// <summary>
    /// 停止跳躍攻擊
    /// </summary>
    public void StopJumpAttack()
    {
        animator.SetBool(AnimatorHash.IsJumpAttack, false);
        if (userController) userController.StopJumpAttack();
    }

    /// <summary>
    /// 受傷
    /// </summary>
    public void Hurt()
    {
        soundRequest.PlaySound("Hurt");

        animator.SetTrigger(AnimatorHash.Hurt_Tr);
        if (userController) userController.OnHurt();
    }

    /// <summary>
    /// 遊戲結果
    /// </summary>
    /// <param name="result">是否獲勝</param>
    public void GameOver(bool result)
    {
        isActionable = false;

        string triggerName = result ? "Win_Tr" : "Die_Tr";
        string clip = result ? "Win" : "Die";
        soundRequest.PlaySound(clip);
        animator.SetTrigger(triggerName);

        if (userController) userController.GameOver(triggerName);
}
}
