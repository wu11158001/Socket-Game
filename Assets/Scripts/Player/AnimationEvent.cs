using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private SoundRequest soundRequest;
    private UserController userController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private Rigidbody2D r2d;
    private BoxCollider2D box2d;

    private SpriteRenderer dashEffect;

    private float initGravity;
    public string userName;

    private float limitPosX = 17;//移動限制範圍

    private bool isActionable = true;//角色是否可行動
    public bool GetActionable { get { return isActionable; } }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        box2d = GetComponent<BoxCollider2D>();

        userController = transform.parent.GetComponent<UserController>();
        soundRequest = transform.parent.GetComponent<SoundRequest>();
        r2d = transform.parent.GetComponent<Rigidbody2D>();   

        dashEffect = transform.parent.Find("DashEffect").GetComponent<SpriteRenderer>();
        dashEffect.gameObject.SetActive(false);

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
                int dir = spriteRenderer.flipX ? 1 : -1;
                transform.parent.position = new Vector3(transform.parent.position.x + 10 * dir * Time.deltaTime, transform.parent.position.y, transform.parent.position.z);
            }
        }

        //動畫完畢執行
        if (stateInfo.normalizedTime >= 1f)
        {
            if (animator.GetBool(AnimatorHash.IsAttack)) StopAttack();
            if (animator.GetBool(AnimatorHash.IsDash)) StopDash();
        }

        //限制移動範圍
        if (transform.parent.position.x > limitPosX) transform.parent.position = new Vector2(limitPosX, transform.parent.position.y);
        else if (transform.parent.position.x < -limitPosX) transform.parent.position = new Vector2(-limitPosX, transform.parent.position.y);
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
    /// <param name="aniHash">動畫Hash</param>
    /// <param name="dir">面相方向(true=右)</param>
    /// <param name="isActive">動畫bool</param>
    public void UpdateAni(int aniHash, bool dir, bool isActive)
    {
        spriteRenderer.flipX = dir;

        if (aniHash == AnimatorHash.Hurt_Tr || aniHash == AnimatorHash.Die_Tr || aniHash == AnimatorHash.Win_Tr)
        {
            animator.SetTrigger(aniHash);
            return;
        }

        animator.SetBool(aniHash, isActive);

        if (aniHash == AnimatorHash.IsDash && isActive == false) StopDash();
    }

    /// <summary>
    /// 播放待機
    /// </summary>
    public void PlayIdle()
    {
        animator.Play(AnimatorHash.IsIdle);
        if (userController) userController.isHurt = false;
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    void OnJump()
    {
        r2d.velocity = new Vector2(0, 25);
    }

    /// <summary>
    /// 翻滾(閃躲)
    /// </summary>
    void OnDash()
    {
        soundRequest.PlaySound("Dash");

        dashEffect.gameObject.SetActive(true);
        float effPosX = spriteRenderer.flipX ? -0.6f : 0.6f;
        float effPosY = -1.03f;
        dashEffect.transform.localPosition = new Vector2(effPosX, effPosY);

        dashEffect.flipX = !spriteRenderer.flipX;

        r2d.gravityScale = 0;
        box2d.enabled = false;

        int forceDir = spriteRenderer.flipX ? 1 : -1;
        r2d.velocity = new Vector2(13 * forceDir, 0);
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    public void StopDash()
    {
        animator.SetBool(AnimatorHash.IsDash, false);

        dashEffect.gameObject.SetActive(false);

        r2d.gravityScale = initGravity;
        box2d.enabled = true;

        if (userController) userController.isDash = false;
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    void OnAttack()
    {
        soundRequest.PlaySound("Attack");

        int forceDir = spriteRenderer.flipX ? 1 : -1;
        r2d.velocity = new Vector2(5 * forceDir, 0);

        if (userController) userController.OpenAttackBox();
    }

    /// <summary>
    /// 停止攻擊
    /// </summary>
    public void StopAttack()
    {
        animator.SetBool(AnimatorHash.IsAttack, false);
        if (userController) userController.isAttack = false;
    }

    /// <summary>
    /// 停止跳躍攻擊
    /// </summary>
    public void StopJumpAttack()
    {
        animator.SetBool(AnimatorHash.IsAttack, false);
        if (userController) userController.isAttack = false;
    }

    /// <summary>
    /// 受傷
    /// </summary>
    public void Hurt()
    {
        soundRequest.PlaySound("Hurt");

        animator.SetTrigger(AnimatorHash.Hurt_Tr);
        if (userController) userController.isHurt = true;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void OnDie()
    {
        soundRequest.PlaySound("Die");
        Destroy(r2d);
        Destroy(box2d);
    }

    /// <summary>
    /// 遊戲結果
    /// </summary>
    /// <param name="result">是否獲勝</param>
    public void GameOver(bool result)
    {
        isActionable = false;

        string clip = result ? "Win" : "Fail";
        soundRequest.PlaySound(clip);

        int triggerHash = result ? AnimatorHash.Win_Tr : AnimatorHash.Die_Tr;
        animator.SetTrigger(triggerHash);
        if (userController) userController.GameOver(triggerHash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator.GetBool(AnimatorHash.IsJump))
        {
            animator.SetBool(AnimatorHash.IsJump, false);
            if (userController) userController.isFloor = true;

            if (animator.GetBool(AnimatorHash.IsAttack))
            {
                animator.SetBool(AnimatorHash.IsAttack, false);
                if (userController) userController.isAttack = false;
            }
        }
    }
}
