using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        if (!IsGroundExist())
        {
            Turn();
        }
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    public void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -moveSpeed, rb.velocity.y);
    }

    private bool IsGroundExist()
    {
        Debug.DrawRay(groundCheckPoint.position, Vector2.down, Color.red);
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, groundLayer);
    }
}
