using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class rhkwpPlayer0523ray : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 inputDir;
    [SerializeField] private float movePower;
    [SerializeField] private float jumpPower;
    [SerializeField] private float maxSpeed;
    private Animator anim;
    private SpriteRenderer renderer;
    public bool isGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        GroundCheck();
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
        anim.SetFloat("Speed", Mathf.Abs(inputDir.x));
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y) - transform.position, Color.red);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
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
