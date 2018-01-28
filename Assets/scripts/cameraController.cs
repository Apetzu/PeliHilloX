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
    Rigidbody playerRb;
    CapsuleCollider playerCollider;

    RaycastHit[] prevRayhits = new RaycastHit[0];

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        startPos = transform.position;      // Camera starting position
    }

    void FixedUpdate ()
    {
        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, player.position + new Vector3 (Mathf.Round(playerRb.velocity.x), 0, Mathf.Round(playerRb.velocity.z)) / 2 + startPos, ref currentVelocity, smoothSpeed);
	}

    /*void Update()
    {
        RaycastHit[] rayHits = Physics.RaycastAll(transform.position, (player.position + Vector3.up * (playerCollider.height / 2) - transform.position).normalized, (player.position - transform.position).magnitude, 1 | 1 << LayerMask.NameToLayer("DisappearObject"));

        //Debug.DrawRay(transform.position, (player.position + Vector3.up * (playerCollider.height / 2) - transform.position));

        if (rayHits.Length != 0)
        {
            if (rayHits != prevRayhits)
            {
                if (prevRayhits.Length != 0)
                {
                    for (int i = 0; i < prevRayhits.Length; i++)
                    {
                        Debug.Log("Puff: " + prevRayhits[i].transform.gameObject);
                        prevRayhits[i].transform.gameObject.layer = 1;
                    }
                }

                for (int i = 0; i < rayHits.Length; i++)
                {
                    rayHits[i].transform.gameObject.layer = LayerMask.NameToLayer("DisappearObject");
                }

                rayHits = prevRayhits;
            }
        }
    }*/
}
