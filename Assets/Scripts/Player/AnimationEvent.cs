using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D r2d;
    private BoxCollider2D box2d;
    private UserController userController;
    private SpriteRenderer spriteRenderer;

    private float initGravity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        r2d = transform.parent.GetComponent<Rigidbody2D>();
        box2d = transform.parent.GetComponent<BoxCollider2D>();
        userController = transform.parent.GetComponent<UserController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initGravity = r2d.gravityScale;
    }

    /// <summary>
    /// 播放待機
    /// </summary>
    void PlayIdle()
    {
        animator.Play(AnimatorHash.IsIdle);
        if (userController) userController.HurtOver();
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
        r2d.gravityScale = 0;
        box2d.enabled = false;

        int forceDir = spriteRenderer.flipX ? 1 : -1;
        r2d.velocity = new Vector2(13 * forceDir, 0);
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    void StopDash()
    {
        animator.SetBool(AnimatorHash.IsDash, false);
        if (userController)
        {
            r2d.gravityScale = initGravity;
            box2d.enabled = true;
            userController.StopDash();
        }
    }

    /// <summary>
    /// 停止攻擊
    /// </summary>
    void StopAttack()
    {
        animator.SetBool(AnimatorHash.IsAttack, false);
        if (userController) userController.StopAttack();
    }

    /// <summary>
    /// 停止跳躍攻擊
    /// </summary>
    void StopJumpAttack()
    {
        animator.SetBool(AnimatorHash.IsJumpAttack, false);
        if (userController) userController.StopJumpAttack();
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    void OnAttack()
    {
        int forceDir = spriteRenderer.flipX ? 1 : -1;
        r2d.velocity = new Vector2(5 * forceDir, 0);

        if (userController) userController.OpenAttackBox();
    }
}
