using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

    [SerializeField]
    float smoothSpeed = 10f;
    [SerializeField]
    Transform player;

    Vector3 currentVelocity = Vector3.zero;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;      // Camera starting position
    }

    void FixedUpdate ()
    {
        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, player.position + startPos, ref currentVelocity, smoothSpeed);
	}

    void Update()
    {
        
    }
}
