using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 inputDir;
    [SerializeField] private float movePower;
    [SerializeField] private float jumpPower;
    [SerializeField] private float maxSpeed;
    private SpriteRenderer renderer;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (inputDir.x < 0 && rb.velocity.x > -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        anim.SetFloat("MoveSpeed", Mathf.Abs(inputDir.x));
        if (inputDir.x > 0)
            renderer.flipX = false;
        if (inputDir.x < 0)
            renderer.flipX = true;
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("IsGround", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("IsGround", false);
    }
}
