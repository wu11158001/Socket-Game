using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateCharacterState : MonoBehaviour
{
    private SpriteRenderer body;
    private Animator animator;
    private Rigidbody2D r2d;
    private BoxCollider2D box2d;

    private float initGravity;

    private void Start()
    {
        animator = transform.Find("Body").GetComponent<Animator>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        r2d = GetComponent<Rigidbody2D>();
        box2d = transform.Find("Body").GetComponent<BoxCollider2D>();

        initGravity = r2d.gravityScale;
    }

    private void Update()
    {
        OnMove();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void OnMove()
    {
        if(!animator.GetBool("IsDash") && !animator.GetBool("IsAttack"))
        {
            if (animator.GetBool("IsRun"))
            {
                int dir = body.flipX ? 1 : -1;
                transform.position = new Vector3(transform.position.x + 10 * dir * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }        
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
        animator.SetBool(aniName, isActive);

        if (aniName == "IsDash" && isActive == false) StopDash();
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    void StopDash()
    {
        r2d.gravityScale = initGravity;
        box2d.enabled = true;
    }
}
