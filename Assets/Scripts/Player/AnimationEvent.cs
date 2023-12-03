using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Rigidbody2D r2d;
    private BoxCollider2D box2d;
    private RoleController roleController;
    private SpriteRenderer spriteRenderer;

    private float initGravity;

    private void Start()
    {
        r2d = transform.parent.GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        roleController = transform.parent.GetComponent<RoleController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initGravity = r2d.gravityScale;
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
        r2d.velocity = new Vector2(11 * forceDir, 0);
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    void StopDash()
    {
        if (roleController)
        {
            r2d.gravityScale = initGravity;
            box2d.enabled = true;
            roleController.StopDash();
        }
    }

    /// <summary>
    /// 停止攻擊
    /// </summary>
    void StopAttack()
    {
        if (roleController) roleController.StopAttack();
    }

    /// <summary>
    /// 停止跳躍攻擊
    /// </summary>
    void StopJumpAttack()
    {
        if (roleController) roleController.StopJumpAttack();
    }
}
