using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    [SerializeField]
    float acceleration = 10f;
    [SerializeField]
    float maxSpeed = 50f;

    Rigidbody rb;                       // Player rigidbody
    Vector3 movDir = Vector3.zero;      // Vector direction where player moves

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        movDir = new Vector3(Input.GetAxisRaw("XMovement"), 0, Input.GetAxisRaw("ZMovement")).normalized;
        rb.AddForce(movDir * acceleration * rb.mass);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
	}
}
