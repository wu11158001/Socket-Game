using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Camera camera;
    private Animator animator;
    private SpriteRenderer body;

    private AttackBox attackBox;
    private UpdateCharacterState updateCharacterState;
    private UpdatePosRequest updatePosRequest;
    private UpdateAinRequest updateAniRequest;

    [SerializeField] private bool isRun;
    [SerializeField] private bool isFloor;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isDash;
    [SerializeField] private bool isHurt;
    [SerializeField] private bool isGameOver;

    private void Start()
    {
        camera = Camera.main;
        animator = transform.Find("Body").GetComponent<Animator>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        attackBox = GetComponentInChildren<AttackBox>();

        updateCharacterState = GetComponent<UpdateCharacterState>();
        updatePosRequest = GetComponent<UpdatePosRequest>();
        updateAniRequest = GetComponent<UpdateAinRequest>();

        InvokeRepeating(nameof(SendUpdatePosFun), 0.1f, 0.1f);
    }

    private void Update()
    {
        //攝影機跟隨
        camera.transform.position = new Vector3(transform.position.x, camera.transform.position.y, camera.transform.position.z);

        if (!isHurt && !isGameOver)
        {
            InputContrl();
        }        
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
    /// 設定動畫
    /// </summary>
    /// <param name="aniName">動畫名稱</param>
    /// <param name="dir">面相方向(true=右)</param>
    /// <param name="isActive">動畫bool</param>
    void SetAni(string aniName, bool dir, bool isActive = true)
    {
        animator.SetBool(aniName, isActive);
        updateAniRequest.SendRequest(aniName, dir, isActive);
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
            SetAni("IsRun", body.flipX);
        }
        //右
        if (Input.GetKey(KeyCode.D) && !isRun)
        {
            isRun = true;
            body.flipX = true;
            SetAni("IsRun", body.flipX);
        }
        //停止移動
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isRun = false;
            SetAni("IsRun", body.flipX, false);
        }

        //跳躍
        if (Input.GetKeyDown(KeyCode.W) && isFloor && !isAttack)
        {
            SetAni("IsJump", body.flipX);
        }

        //翻滾(閃躲)
        if (Input.GetKeyDown(KeyCode.K) && isFloor && !isAttack)
        {
            isDash = true;
            SetAni("IsDash", body.flipX);
        }

        //攻擊
        if (Input.GetKeyDown(KeyCode.J) && !isAttack && !isDash)
        {
            isAttack = true;
            if (isFloor) SetAni("IsAttack", body.flipX);
            else SetAni("IsJumpAttack", body.flipX);
        }
    }

    /// <summary>
    /// 停止翻滾(閃躲)
    /// </summary>
    public void StopDash()
    {
        isDash = false;
        SetAni("IsDash", body.flipX, false);
    }

    /// <summary>
    /// 停止攻擊
    /// </summary>
    public void StopAttack()
    {
        isAttack = false;
        SetAni("IsAttack", body.flipX, false);
    }

    /// <summary>
    /// 停止跳躍攻擊
    /// </summary>
    public void StopJumpAttack()
    {
        isAttack = false;
        SetAni("IsJumpAttack", body.flipX, false);
    }

    /// <summary>
    /// 開啟攻擊框
    /// </summary>
    public void OpenAttackBox()
    {
        attackBox.OpenBox(body.flipX);
    }

    /// <summary>
    /// 受傷
    /// </summary>
    public void OnHurt()
    {
        isHurt = true;
    }

    /// <summary>
    /// 受擊狀態結束
    /// </summary>
    public void HurtOver()
    {
        isHurt = false;
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    /// <param name="triggerName"></param>
    public void GameOver(string triggerName)
    {
        isGameOver = true;
        updateAniRequest.SendRequest(triggerName, body.flipX, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFloor)
        {
            isFloor = true;
            SetAni("IsJump", body.flipX, false);
            if (isAttack)
            {
                isAttack = false;
                SetAni("IsJumpAttack", body.flipX, false);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isFloor = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isFloor = false;
    }
}
