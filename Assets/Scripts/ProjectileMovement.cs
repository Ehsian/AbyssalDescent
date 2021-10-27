using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public string projectileType;

    float speed;
    public float sizeX;
    public float sizeY;
    Vector3 dir;

    Vector3 mouse;

    bool left;
    float t;
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
                t = Time.time;
                if (left)
                {
                    transform.position +=
                        transform.right * -1f;
                    transform.localScale = new Vector2(-sizeX, sizeY);
                }
                else
                {
                    transform.position +=
                        transform.right;
                    transform.localScale = new Vector2(5f, 5f);
                }
                speed = 2;
                
                Destroy(this.gameObject, 5f);
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
                    transform.localScale = new Vector2(-sizeX, sizeY);
                }
                else
                {
                    transform.position +=
                        transform.right * speed * Time.deltaTime;
                    transform.localScale = new Vector2(sizeX, sizeY);
                }
                break;
            case "Water":
                if (Time.time - t >= 1.5f) {
                    speed = 6f;
                }
                if (left)
                {
                    transform.position +=
                        transform.right * -speed * Time.deltaTime;
                    transform.localScale = new Vector2(-sizeX, sizeY);
                }
                else
                {
                    transform.position +=
                        transform.right * speed * Time.deltaTime;
                    transform.localScale = new Vector2(sizeX, sizeY);
                }
                break;
            default:
                speed = 0;
                break;
        }
    }
}
