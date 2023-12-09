using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    private Camera mainCamera;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private AttackBox attackBox;
    private AnimationEvent animationEvent;
    private UpdatePosRequest updatePosRequest;
    private UpdateAinRequest updateAniRequest;

    public bool isRun;
    public bool isFloor;
    public bool isAttack;
    public bool isDash;
    public bool isHurt;
    [SerializeField] private bool isActionable = true;

    private void Start()
    {
        mainCamera = Camera.main;
        Transform body = transform.Find("Body");

        animator = body.GetComponent<Animator>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        animationEvent = body.GetComponent<AnimationEvent>();

        attackBox = GetComponentInChildren<AttackBox>();       
        updatePosRequest = GetComponent<UpdatePosRequest>();
        updateAniRequest = GetComponent<UpdateAinRequest>();

        InvokeRepeating(nameof(SendUpdatePosFun), 0.5f, 0.1f);
    }

    private void Update()
    {
        //攝影機跟隨
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

        if (!isHurt && isActionable)
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
    /// <param name="aniHash">動畫Hash</param>
    /// <param name="dir">面相方向(true=右)</param>
    /// <param name="isActive">動畫bool</param>
    void SetAni(int aniHash, bool dir, bool isActive = true)
    {
        animator.SetBool(aniHash, isActive);
        updateAniRequest.SendRequest(aniHash, dir, isActive);
    }

    /// <summary>
    /// 輸入控制
    /// </summary>
    private void InputContrl()
    {
        //左
        if (Input.GetKey(KeyCode.LeftArrow) && !isRun)
        {
            isRun = true;
            spriteRenderer.flipX = false;
            SetAni(AnimatorHash.IsRun, spriteRenderer.flipX);
        }
        //右
        if (Input.GetKey(KeyCode.RightArrow) && !isRun)
        {
            isRun = true;
            spriteRenderer.flipX = true;
            SetAni(AnimatorHash.IsRun, spriteRenderer.flipX);
        }
        //停止移動
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRun = false;
            SetAni(AnimatorHash.IsRun, spriteRenderer.flipX, false);
        }

        //跳躍
        if (Input.GetKeyDown(KeyCode.UpArrow) && isFloor && !isAttack)
        {
            SetAni(AnimatorHash.IsJump, spriteRenderer.flipX);
        }

        //翻滾(閃躲)
        if (Input.GetKeyDown(KeyCode.X) && isFloor && !isAttack)
        {
            isDash = true;
            SetAni(AnimatorHash.IsDash, spriteRenderer.flipX);
        }

        //攻擊
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack && !isDash)
        {
            isAttack = true;
            SetAni(AnimatorHash.IsAttack, spriteRenderer.flipX);
        }
    }
    
    /// <summary>
    /// 開啟攻擊框
    /// </summary>
    public void OpenAttackBox()
    {
        attackBox.OpenBox(spriteRenderer.flipX);
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    /// <param name="aniHash"></param>
    public void GameOver(int aniHash)
    {
        isActionable = false;
        SetAni(aniHash, spriteRenderer.flipX);
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
