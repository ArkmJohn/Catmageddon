using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CatInfo))]
public class CatMovement : MonoBehaviour {

    Rigidbody rb;
    [Tooltip("Speed for Tanks. Changes based on the tank equipped")]
    public float speed = 2f;
    [Tooltip("Turn speed for tank. Mostly uniform for all tanks")]
    public float turnSpeed = 90f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //if (PhotonNetwork.player.IsLocal)
        //{
        //    FindObjectOfType<CameraControl>().localPlayer = this.gameObject.transform;
        //}
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.player.IsLocal && this.GetComponent<CatInfo>().Health > 0)
        {
            Move();
        }
    }

    // Moves the Player
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * vertical * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        float turn = horizontal * turnSpeed * Time.deltaTime;
        Quaternion turnRot = Quaternion.Euler(0, turn, 0);

        rb.MoveRotation(rb.rotation * turnRot);
    }

    // TODO: Move the turret of the player
}
