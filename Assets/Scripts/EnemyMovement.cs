using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float initSpeed = -2;

    float move = 1.5f;
    public float maxEnemyHP = 10f;
    float enemyHP = 10f;
    public Slider hpBar;
    Vector3 hpPos;

    // Start is called before the first frame update
    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector2(-0.5f, 0.4f);
        enemyHP = maxEnemyHP;
    }

    void Update()
    {
        rb.freezeRotation = true;
        rb.velocity = new Vector2(move, rb.velocity.y);
        hpBar.value = enemyHP/maxEnemyHP;
        hpPos = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, 0f));
        hpBar.transform.position = hpPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "L")
        {
            move = initSpeed;
            transform.localScale = new Vector2(-0.5f, 0.4f);
        }
        if (other.tag == "R")
        {
            move = -initSpeed;
            transform.localScale = new Vector2(0.5f, 0.4f);
        }
    }
}
