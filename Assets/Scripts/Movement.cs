using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 5;

    public float jumpForce = 4;

    bool isGrounded = false;

    public float fallMultiplier = 8;

    public float lowJumpMultiplier = 5;

    public Transform isGroundedChecker;

    public float checkGroundRadius = 0.05f;

    public LayerMask groundLayer;

    public float rememberGroundedFor = 0.1f;

    float lastTimeGrounded;

    float lastTimeDamaged;

    private float jumpChargeTime;

    public float maxJumpMultiplier = 2;

    Transform transfrm;

    public Transform tpexit;
    [SerializeField]
    float maxHP = 5f;

    public Slider hpBar;

    float hp = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(7.85f, 65.5f, 0f);
        rb = GetComponent<Rigidbody2D>();
        transfrm = GetComponent<Transform>();
        lastTimeDamaged = Time.time;
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        CheckIfGrounded();
        //Debug.Log(Mathf.Floor(rb.velocity.y));
        hpBar.value = hp/maxHP;
    }

    void Move()
    {
        rb.freezeRotation = true;
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(moveBy / 3, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveBy, rb.velocity.y);
        }
    }

    void Jump()
    {
        if (
            Input.GetKey(KeyCode.Space) &&
            (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)
        )
        {
            if (jumpChargeTime < maxJumpMultiplier)
            {
                jumpChargeTime += Time.deltaTime;
                transform.localScale += new Vector3(0.0015f, -0.0015f);
            }
        }
        else if (
            Input.GetKeyUp(KeyCode.Space) &&
            (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)
        )
        {
            rb.velocity =
                new Vector2(rb.velocity.x, jumpForce * (1 + jumpChargeTime));
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Invis")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (other.tag == "Vis")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Time.time - lastTimeDamaged > 0.1f)
            {
                Debug.Log("HIT");
                hp--;
                lastTimeDamaged = Time.time;
            }
        }
    }
}
