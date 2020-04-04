using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        Debug.Log("Start des Scripts!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
             playerBody.AddForce(Vector3.left*speed);
         if (Input.GetKey(KeyCode.D))
             playerBody.AddForce(Vector3.right*speed);
         if (Input.GetKey(KeyCode.W))
             playerBody.AddForce(Vector3.up*speed);
         if (Input.GetKey(KeyCode.S))
             playerBody.AddForce(Vector3.down*speed);
    }
}
