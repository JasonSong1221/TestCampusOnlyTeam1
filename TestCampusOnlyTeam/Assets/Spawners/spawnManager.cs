using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnObjects;
    [SerializeField] public int numToSpawn;

    bool isSpawning;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator spawn()
    {
        isSpawning = true;

    }

}
