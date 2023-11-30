using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D r2d;
    private bool isFloor;

    private float moveSpeed = 20;
    private float jumpForce = 30;

    private void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");

        if(h != 0)
        {
            r2d.velocity = new Vector2(h * moveSpeed, r2d.velocity.y);
        }
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.W) && isFloor == false)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpForce);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isFloor = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isFloor = false;
    }
}
