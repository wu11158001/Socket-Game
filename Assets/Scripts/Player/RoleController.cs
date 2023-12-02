using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer body;

    private UpdatePosRequest updatePosRequest;
    private UpdateAinRequest updateAniRequest;

    private bool isRun;

    private void Start()
    {
        animator = transform.Find("Body").GetComponent<Animator>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();

        updatePosRequest = GetComponent<UpdatePosRequest>();
        updateAniRequest = GetComponent<UpdateAinRequest>();

        InvokeRepeating("SendUpdatePosFun", 0.5f, 0.1f);
    }

    private void Update()
    {
        InputContrl();
    }

    /// <summary>
    /// 發送更新位置
    /// </summary>
    void SendUpdatePosFun()
    {
        Vector2 pos = transform.position;
        updatePosRequest.SendRequest(pos);
    }

    /// <summary>
    /// 輸入控制
    /// </summary>
    private void InputContrl()
    {
        //左
        if (Input.GetKey(KeyCode.A) && !isRun)
        {
            isRun = true;
            body.flipX = false;
            animator.SetBool("IsRun", true);

            updateAniRequest.SendRequest("IsRun", body.flipX);
            
        }
        //右
        if (Input.GetKey(KeyCode.D) && !isRun)
        {
            isRun = true;
            body.flipX = true;
            animator.SetBool("IsRun", true);

            updateAniRequest.SendRequest("IsRun", body.flipX);
        }
        //停止移動
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            isRun = false;
            animator.SetBool("IsRun", false);

            updateAniRequest.SendRequest("IsRun", body.flipX, false);
        }
    }
}
