using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    [SerializeField]
    float acceleration = 10f;
    [SerializeField]
    float maxSpeed = 50f;
    [SerializeField]
    bool flipXAxis = false;
    [SerializeField]
    bool flipZAxis = false;

    Rigidbody rb;                       // Player rigidbody
    Vector3 movDir = Vector3.zero;      // Vector direction where player moves

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        // Direction where player moves, axes flipping
        movDir = new Vector3(Input.GetAxisRaw("XMovement") * (flipXAxis ? -1 : 1), 0, Input.GetAxisRaw("ZMovement") * (flipZAxis ? -1 : 1)).normalized;

        // Movement changes (applying force)
        rb.AddForce(movDir * acceleration * rb.mass);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (movDir != Vector3.zero)
        {
            if (movDir.x > 1)
            {

            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Normie")
        {
            collision.gameObject.GetComponent<NormieAI>().ChangeToInfected();
        }
    }
}
