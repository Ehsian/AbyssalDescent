using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    //Settings
    public int fps = 60;

    //Object
    Rigidbody2D rb;

    Transform transfrm;

    //Movement
    public float speed = 5;

    public float jumpForce = 4;

    //Jumping
    bool isGrounded = false;

    public float hardFallMultiplier = 3;

    public Transform isGroundedChecker;

    public float checkGroundRadius = 0.05f;

    public LayerMask groundLayer;

    public float rememberGroundedFor = 0.1f;

    float lastTimeGrounded;

    private float jumpChargeTime;

    public float maxJumpMultiplier = 2;

    public float squashAndStretch = 0.0015f;

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

    float lastFire;

    float fireTime = 0.35f;

    public GameObject wave;

    float lastWater;

    float waterTime = 0.75f;

    //Abyss
    public float lowestHeight = 65.5f;

    public GameObject poisonCloud;

    float poisonDiff = 262.5f;

    //HUD
    public string slot1 = "";

    public string slot2 = "";

    public string slot3 = "";

    public string slot4 = "";

    public GameObject[] slot1Icon;

    public GameObject[] slot2Icon;

    public GameObject[] slot3Icon;

    public GameObject[] slot4Icon;

    float lastSwapped = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fps;
        transform.position = new Vector3(12.5f, 60.5f, 0f);
        lowestHeight = 65.5f;
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
        OtherFunctions();
        ChangeWeapon();
        HUD();
    }

    void ChangeWeapon()
    {
        if (Time.time - lastSwapped > 0.15f)
        {
            lastSwapped = Time.time;
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                string temp = slot4;
                slot4 = slot3;
                slot3 = slot2;
                slot2 = slot1;
                slot1 = temp;
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                string temp = slot1;
                slot1 = slot2;
                slot2 = slot3;
                slot3 = slot4;
                slot4 = temp;
            }
        }
    }

    void HUD()
    {
        for (int i = 0; i < slot1Icon.Length; i++)
        {
            slot1Icon[i].SetActive(false);
            slot2Icon[i].SetActive(false);
            slot3Icon[i].SetActive(false);
            slot4Icon[i].SetActive(false);
        }
        if (slot1 == "Fire") slot1Icon[0].SetActive(true);
        if (slot1 == "Water") slot1Icon[1].SetActive(true);

        if (slot2 == "Fire") slot2Icon[0].SetActive(true);
        if (slot2 == "Water") slot2Icon[1].SetActive(true);

        if (slot3 == "Fire") slot3Icon[0].SetActive(true);
        if (slot3 == "Water") slot3Icon[1].SetActive(true);

        if (slot4 == "Fire") slot4Icon[0].SetActive(true);
        if (slot4 == "Water") slot4Icon[1].SetActive(true);
    }

    void OtherFunctions()
    {
        if (hp <= 0) SceneManager.LoadScene("Game");
        hpBar.value = hp / maxHP;
        lowestHeight =
            transform.position.y < lowestHeight
                ? transform.position.y
                : lowestHeight;
        poisonCloud.transform.position =
            new Vector2(0, lowestHeight + poisonDiff);
    }

    void Move()
    {
        rb.freezeRotation = true;
        float x = Input.GetAxisRaw("Horizontal");

        if (x < 0)
        {
            recentlyLeft = true;
        }
        if (x > 0)
        {
            recentlyLeft = false;
        }

        float moveBy = x * speed;
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(moveBy / 3, rb.velocity.y);
        }
        else if (!isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity =
                    new Vector2(moveBy / 5f,
                        (
                        rb.velocity.y < 0f
                            ? rb.velocity.y * hardFallMultiplier
                            : Mathf.Abs(rb.velocity.y)
                        ));
            }
            else
            {
                rb.velocity = new Vector2(moveBy / 1.35f, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(moveBy, rb.velocity.y);
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (slot1)
            {
                case "Fire":
                    if (Time.time - lastFire > fireTime)
                    {
                        lastFire = Time.time;
                        GameObject fire =
                            (GameObject)
                            Instantiate(fireball,
                            transform.position,
                            Quaternion.identity);
                    }
                    break;
                case "Water":
                    if (Time.time - lastWater > waterTime)
                    {
                        lastWater = Time.time;
                        GameObject water =
                            (GameObject)
                            Instantiate(wave,
                            transform.position,
                            Quaternion.identity);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void Jump()
    {
        if (Mathf.Abs(vel.y) > 0.5)
        {
            transform.localScale =
                new Vector3(0.98f -
                    Mathf.Min(Mathf.Abs(vel.y), 15) * squashAndStretch * 15,
                    0.98f +
                    Mathf.Min(Mathf.Abs(vel.y), 1) * squashAndStretch * 15);
            return;
        }
        if (
            Input.GetKey(KeyCode.Space) &&
            (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)
        )
        {
            if (jumpChargeTime < maxJumpMultiplier)
            {
                jumpChargeTime += Time.deltaTime;
                transform.localScale +=
                    new Vector3(squashAndStretch, -squashAndStretch);
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
        }
        else if (transform.localScale != new Vector3(1, 1))
        {
            if (
                Mathf.Round(transform.localScale.x) == 1 &&
                Mathf.Round(transform.localScale.y) == 1
            )
            {
                transform.localScale = new Vector3(0.98f, 0.98f);
                //Debug.Log("Ki3");
            }
            else if (transform.localScale.x < 1)
            {
                transform.localScale =
                    new Vector3(transform.localScale.x + squashAndStretch,
                        transform.localScale.y - squashAndStretch);
                //Debug.Log("Ki");
            }
            else
            {
                transform.localScale =
                    new Vector3(transform.localScale.x - squashAndStretch,
                        transform.localScale.y + squashAndStretch);
                //Debug.Log("Ki2");
            }
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
            if (
                Mathf.Abs(Mathf.Floor(vel.y)) > 12f &&
                (Time.time - lastTimeDamaged > 0.5f)
            )
            {
                print(Mathf.Abs(Mathf.Floor(vel.y)));
                if (Mathf.Abs(Mathf.Floor(vel.y)) < 14f)
                {
                    hp -= 1;
                }
                else if (Mathf.Abs(Mathf.Floor(vel.y)) < 16f)
                {
                    hp -= 2;
                }
                else if (Mathf.Abs(Mathf.Floor(vel.y)) < 18f)
                {
                    hp -= 3;
                }
                else if (Mathf.Abs(Mathf.Floor(vel.y)) < 20f)
                {
                    hp -= 4;
                }
                else
                {
                    hp -= 10;
                }
                lastTimeDamaged = Time.time;
            }
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
            if (
                jumpChargeTime != 0 &&
                Time.time - lastTimeGrounded > rememberGroundedFor
            )
            {
                jumpChargeTime = 0;
                transform.localScale = new Vector3(0.98f, 0.98f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "LowGravity")
        {
            if (vel.y < -10)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 2);
            }
        }
    }

    void FixedUpdate()
    {
        vel = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Mathf.Abs(Mathf.Floor(vel.y)) >= squishThreshold)
            {
                EnemyMovement enemy =
                    other.gameObject.GetComponent<EnemyMovement>();
                enemy
                    .TakeDamage(Mathf.Abs(Mathf.Floor(vel.y / 3)),
                    "being squished");
            }
            else if (Time.time - lastTimeDamaged > 0.1f)
            {
                Debug.Log("HIT");
                hp--;
                lastTimeDamaged = Time.time;
            }
        }
        if (other.gameObject.tag == "Breakable")
        {
            if (Mathf.Abs(Mathf.Floor(vel.y)) >= 10)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "Hazard")
        {
            if (Time.time - lastTimeDamaged > 0.5f)
            {
                Debug.Log("Impaled");
                hp--;
                lastTimeDamaged = Time.time;
            }
        }
    }
}
