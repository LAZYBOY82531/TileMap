using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rhkwpRat0523 : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
    }
}
