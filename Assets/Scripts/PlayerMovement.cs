using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody rb;

    public float speed = 2f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * vertical * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        float turn = horizontal * speed * Time.deltaTime;
        Quaternion turnRot = Quaternion.Euler(0, turn, 0);

        rb.MoveRotation(rb.rotation * turnRot);
    }

}
