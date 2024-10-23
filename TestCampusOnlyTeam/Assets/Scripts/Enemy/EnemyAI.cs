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
        gamemanager.instance.updateGameGoal(1);
        
        
    }

   

    

    // Update is called once per frame
    void Update()
    {

        playerDir = gamemanager.instance.player.transform.position - transform.position;
        agent.SetDestination(gamemanager.instance.player.transform.position);
        if (playerInRange)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }


        }


    }



    void faceTarget()
    {
        Vector3 look;
        look.x = playerDir.x;
        look.z = playerDir.z;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    
    public void takeDamage(int damage, Vector3 impulsePosition)
    {
        if (health <= 0) return; // Prevent further damage after death

        health -= damage;
        //GetComponent<Rigidbody>().AddExplosionForce(15, impulsePosition, 20, 0);
        StartCoroutine(flashColor());

        if (health <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Die();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
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
        yield return new WaitForSeconds(0.25f);
        model.material.color = colorOrig;
    }

    
}
