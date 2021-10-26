using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{
    public Text text;
    public string words;
    public GameObject textbox;
    private bool inRange = false;

    public GameObject player;
    private Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.S))
        {
            if (textbox.activeInHierarchy)
            {
                textbox.SetActive(false);
                movement.enabled = true;
            }
            else
            {
                text.text = words;
                textbox.SetActive(true);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                movement.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
