using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    //Object
    Rigidbody2D rb;
    Transform transfrm;

    //Movement
    public float speed = 5;

    public float jumpForce = 4;

    //Jumping
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

    //Cinematic
    public Transform tpexit;

    //Combat
    float lastTimeDamaged;

    [SerializeField]
    float maxHP = 5f;

    public Slider hpBar;

    float hp = 5f;
    public bool recentlyLeft;
    float squishThreshold = 6;
    Vector3 vel;
    //Weapons
    public GameObject fireball;
    public GameObject wave;

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
        Shoot();
        //Debug.Log(Mathf.Floor(rb.velocity.y));
        hpBar.value = hp/maxHP;
    }

    void Move()
    {
        rb.freezeRotation = true;
        float x = Input.GetAxisRaw("Horizontal");

        if (x < 0) {
            recentlyLeft = true;
        }
        if (x > 0) {
            recentlyLeft = false;
        }

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

    void Shoot() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            GameObject fire = (GameObject) Instantiate(fireball, transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            GameObject water = (GameObject) Instantiate(wave, transform.position, Quaternion.identity);
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

    void FixedUpdate() {
        vel = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Mathf.Abs(Mathf.Floor(vel.y)) >= squishThreshold) {
                EnemyMovement enemy = other.gameObject.GetComponent<EnemyMovement>();
                enemy.TakeDamage(Mathf.Abs(Mathf.Floor(vel.y / 3)), "being squished");
            }
            else if (Time.time - lastTimeDamaged > 0.1f)
            {
                Debug.Log("HIT");
                hp--;
                lastTimeDamaged = Time.time;
            }
        }
    }
}
