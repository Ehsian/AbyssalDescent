using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float initSpeed = 2;
    float speed = 2;

    float move = 1;

    // Start is called before the first frame update
    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = speed * initSpeed;
        transform.localScale = new Vector2(speed, 1f);

        rb.freezeRotation = true;
        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "L")
        {
            speed = initSpeed == Mathf.Abs(initSpeed) ? 1 : -1;
        }
        if (other.tag == "R")
        {
            speed = initSpeed == Mathf.Abs(initSpeed) ? -1 : 1;
        }
    }
}
