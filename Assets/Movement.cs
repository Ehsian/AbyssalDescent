using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 5;

    public float jumpForce = 8;

    bool isGrounded = false;

    public float fallMultiplier = 8;

    public float lowJumpMultiplier = 5;

    public Transform isGroundedChecker;

    public float checkGroundRadius = 0.05f;

    public LayerMask groundLayer;

    public float rememberGroundedFor = 0.1f;

    float lastTimeGrounded;

    private float jumpChargeTime;

    public float maxJumpMultiplier = 2;

    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        CheckIfGrounded();
    }

    void Move()
    {
        rb.freezeRotation = true;
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            if (jumpChargeTime < maxJumpMultiplier)
            {
                jumpChargeTime += Time.deltaTime;
                transform.localScale += new Vector3(0.0005f, -0.0005f);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * (1 + jumpChargeTime));
            jumpChargeTime = 0;
            transform.localScale = new Vector3(1, 1);
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders =
            Physics2D
                .OverlapCircle(isGroundedChecker.position,
                checkGroundRadius,
                groundLayer);
        if (colliders != null)
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
}