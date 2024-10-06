using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    List<Vector3> MonsterPositionCache = new List<Vector3>();
    List<Quaternion> MonsterRotationCache = new List<Quaternion>();

    [SerializeField] GameObject dummy;

    float timeScaleOrig;
    GameObject player;

    bool dirtyCache;

    void Awake()
    {
        clearCache();
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        dirtyCache = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            killAllMonsters();
            Debug.Log("FUCK");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            resetMonsters();
        }

        if(dirtyCache)
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("monster");

            foreach (GameObject monster in gos)
            {
                pushMonster(monster);
            }
            dirtyCache = false;
        }
    }

    void clearCache()
    {
        MonsterPositionCache.Clear();
        MonsterRotationCache.Clear();


    }

    

    public void pushMonster(GameObject monster)
    {
        Vector3 pos = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z);
        Quaternion quat = new Quaternion(monster.transform.rotation.x, monster.transform.rotation.y, monster.transform.rotation.z, monster.transform.rotation.w);
        MonsterPositionCache.Add(pos);
        MonsterRotationCache.Add(quat);

    }

    void killAllMonsters()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("monster");

        foreach (GameObject monster in gos)
        {
            IMonster mon = monster.GetComponent<IMonster>();
            mon.Die();
        }
    }

    void resetMonsters()
    {
        killAllMonsters();
        for(int i = 0; i < MonsterPositionCache.Count; ++i)
        {
            Instantiate(dummy, MonsterPositionCache[i], MonsterRotationCache[i]);
        }
    }

    private gamemanager()
    {

    }

    
}
