using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerNormie : MonoBehaviour 
{
    private NavMeshAgent agent;
    public float spawnLimit;
    float spawnedAmount;
    public GameObject Normie;
    public float spawnRadius;
	void Start () 
    {
        agent = GetComponent<NavMeshAgent> ();

        for (int i = 0; i < spawnLimit; i++)
        {
            Vector3 newPos = RandomNavSphere (transform.position, spawnRadius, -1);
            Instantiate(Normie, newPos, Normie.transform.rotation);
        }
	}
        
    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
