using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public string projectileType;

    float speed;

    Vector3 dir;

    Vector3 mouse;

    bool left;

    float t;

    Vector3 startPosition;

    Vector3 target;

    void Start()
    {
        Movement moo = GameObject.Find("Player").GetComponent<Movement>();
        left = moo.recentlyLeft;
        switch (projectileType)
        {
            case "Fire":
                speed = 10;
                Destroy(this.gameObject, 5f);
                break;
            case "Water":
                speed = 5;
                Destroy(this.gameObject, 7.5f);
                break;
            default:
                speed = 0;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (projectileType)
        {
            case "Fire":
                if (left)
                {
                    transform.position +=
                        transform.right * -speed * Time.deltaTime;
                    transform.localScale = new Vector2(-5, 5);
                }
                else
                {
                    transform.position +=
                        transform.right * speed * Time.deltaTime;
                    transform.localScale = new Vector2(5, 5);
                }
                break;
            case "Water":
                if (left)
                {
                    transform.position +=
                        transform.right * -speed * Time.deltaTime;
                    transform.localScale = new Vector2(-5f, 5f);
                }
                else
                {
                    transform.position +=
                        transform.right * speed * Time.deltaTime;
                    transform.localScale = new Vector2(5f, 5f);
                }
                break;
            default:
                speed = 0;
                break;
        }
    }
}
