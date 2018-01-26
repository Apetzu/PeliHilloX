using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    [SerializeField]
    float acceleration = 10f;
    [SerializeField]
    float maxSpeed = 50f;

    Rigidbody rb;
    Vector3 movDir = Vector3.zero;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        movDir = new Vector3(Input.GetAxisRaw("XMovement"), 0, Input.GetAxisRaw("ZMovement")).normalized;
        rb.AddForce(movDir * acceleration * rb.mass);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
	}
}
