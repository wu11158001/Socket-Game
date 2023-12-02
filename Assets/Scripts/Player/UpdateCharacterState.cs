using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtobuf;

public class UpdateCharacterState : MonoBehaviour
{
    private SpriteRenderer body;
    private Animator animator;
    private AnimatorStateInfo stateInfo;

    private void Start()
    {
        animator = transform.Find("Body").GetComponent<Animator>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        OnMove();
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void OnMove()
    {
        if (stateInfo.IsName("Run"))
        {
            int dir = body.flipX ? 1 : -1;
            transform.position = new Vector3(transform.position.x + 10 * dir * Time.deltaTime, transform.position.y, transform.position.z);
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
    /// <param name="isActive">動畫bool</param>
    /// <param name="dir">面相方向(true=左)</param>
    public void UpdateAni(string aniName, bool isActive, bool dir)
    {
        body.flipX = dir;
        animator.SetBool(aniName, isActive);
    }
}
