using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    private CapsuleCollider bodyCollider;
    private Rigidbody bodyRB;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        bodyCollider = GetComponent<CapsuleCollider>();
        bodyRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float updown = Input.GetAxis("Vertical");
        float leftright = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(leftright, 0.0f, updown);
        movement *= moveSpeed;

        controller.Move(movement);

        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0.0f, -0.5f, 0.0f));
        }
    }
}
