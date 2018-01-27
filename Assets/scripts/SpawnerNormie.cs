﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnerNormie : MonoBehaviour 
{
    [SerializeField]
    float spawnLimit;
    [SerializeField]
    GameObject Normie;
    [SerializeField]
    float MapXSize = 10;
    [SerializeField]
    float MapZSize = 10;
    [SerializeField]
    float navMeshSearchDist = 10;
    public Text PeopleCount;
    int InfectedAmount;
    int NormieAmount;

    void Start () 
    {
        Debug.DrawLine(new Vector3(transform.position.x - MapXSize / 2, 1, 0), new Vector3(transform.position.x + MapXSize / 2, 1, 0), Color.red, 10f);
        Debug.DrawLine(new Vector3(0, 1, transform.position.z - MapZSize / 2), new Vector3(0, 1, transform.position.z + MapZSize / 2), Color.blue, 10f);

        for (int i = 0; i < spawnLimit; i++)
        {
            Vector3 newPos = RandomNavPos(MapXSize, MapZSize, navMeshSearchDist, -1);
            Instantiate(Normie, newPos, Normie.transform.rotation);
        }

	}
    void Update()
    {
        InfectedAmount = GameObject.FindGameObjectsWithTag("Infected").Length;
        NormieAmount = GameObject.FindGameObjectsWithTag("Normie").Length;
        PeopleCount.text = "Infected: " + InfectedAmount + "     Normies: "+ NormieAmount;
    }
    Vector3 RandomNavPos(float xSize, float zSize, float dist, int layermask)
    {
        NavMeshHit navHit;
        NavMesh.SamplePosition(new Vector3(Random.Range(transform.position.x - xSize / 2, transform.position.x + xSize / 2), 0,
                                           Random.Range(transform.position.z - zSize / 2, transform.position.z + zSize / 2)), out navHit, dist, layermask);
        return navHit.position;
    }
}
