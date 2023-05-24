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
    [SerializeField] LayerMask groundLayer;
    private SpriteRenderer renderer;
    private Animator anim;
    public bool isGround;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        GroundCheck();
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

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y) - transform.position, Color.red);
        if(hit.collider != null)
        {
            isGround = true;
            anim.SetBool("IsGround", true);
        }
        else
        {
            isGround = false;
            anim.SetBool("IsGround", false);
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.red);
        }
    }
}
