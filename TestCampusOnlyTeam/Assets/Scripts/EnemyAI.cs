using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IMonster
{
    [SerializeField] int health;

    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] NavMeshAgent agent;

    bool isShooting;

    [SerializeField] Renderer model;
    Color colorOrig;

    [SerializeField] int rotationSpeed;
    Vector3 playerDir;

    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        //gameManager.instance.updateGameGoal(1);
        
        
    }

    void Awake()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {



        
    }



    void faceTarget()
    {
        Vector3 look;
        look.x = playerDir.x;
        look.z = playerDir.z;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }


    public void takeDamage(int damage, Vector3 impulsePosition)
    {
        health -= damage;
        GetComponent<Rigidbody>().AddExplosionForce(75, impulsePosition, 20, 0);
        StartCoroutine(flashColor());
        if (health <= 0)
        {
            //gameManager.instance.updateGameGoal(-1);
            //Destroy(gameObject);
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public string Type()
    {
        return "dummy";
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

}
