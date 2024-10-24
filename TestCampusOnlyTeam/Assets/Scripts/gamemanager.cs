using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    List<Vector3> MonsterPositionCache = new List<Vector3>();
    List<Quaternion> MonsterRotationCache = new List<Quaternion>();

    Vector3 playerPositionCache;
    Quaternion playerRotationCache;
    
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] TMP_Text enemyCountText;

    public Image playerHpBar;

    public GameObject playerDamageScreen;
    public PlayerController playerScript;
    public GameObject playerSpawnPOS;
    public TMP_Text ammoCur, ammoMax;
    [SerializeField] GameObject dummy;

    float timeScaleOrig;
    public GameObject player;

    public bool isPaused;

    int enemyCount;

    bool dirtyCache;

    void Awake()
    {
        clearCache();
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPOS = GameObject.FindWithTag("PlayerSpawnPOS");
        dirtyCache = true;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            killAllMonsters();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            resetMonsters();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            resetPlayer();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            resetPlayer();
            resetMonsters();
        }

        if (dirtyCache)
        {
            cachePlayer();

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


    void cachePlayer()
    {
        playerPositionCache = player.transform.position;
        playerRotationCache = player.transform.rotation;
    }

    void resetPlayer()
    {
        player.transform.position = playerPositionCache;
        player.transform.rotation = playerRotationCache;
    }

    void pushMonster(GameObject monster)
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

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void updateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoCur.text = currentAmmo.ToString();
        ammoMax.text = maxAmmo.ToString();
    }
    public void ShowWinMenu()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public int GetEnemyCount()
    {
        return enemyCount;
    }

    private gamemanager()
    {

    }
}
