using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(0, player.transform.position.y,this.transform.position.z);
    }
}
