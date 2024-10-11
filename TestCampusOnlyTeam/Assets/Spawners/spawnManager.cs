using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnObjects;
    [SerializeField] public int numToSpawn;
    [SerializeField] float spawnTime;
    [SerializeField] Transform[] spawnPos;

    public int spawnCount;

    bool isSpawning;
    bool startSpawning;
    public bool isObjectiveSpawner;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            StartCoroutine
        }
    }


    IEnumerator spawn()
    {
        isSpawning = true;

    }

}
