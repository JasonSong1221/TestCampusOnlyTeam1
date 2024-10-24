using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossAi : MonoBehaviour, IDamage, IMonster
{
    [SerializeField] int health;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Renderer model;
    [SerializeField] int rotationSpeed;
    [SerializeField] float walkPointRange;
    [SerializeField] LayerMask groundMask, playerMask;
    [SerializeField] float sightRange, attackRange;

    bool isShooting, isWalking, playerInRange, playerInSight;
    Vector3 walkPoint, playerDir;
    Color colorOrig;

    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSight && !playerInRange) Patrol();
        if (playerInSight && !playerInRange) Chase();
        if (playerInSight && playerInRange) Attacking();
    }

    void Patrol()
    {
        if (!isWalking) SearchWalkroad();

        if (isWalking)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        if (distanceToWalkPoint.magnitude < 1f)
        {
            isWalking = false;
        }
    }

    void SearchWalkroad()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            isWalking = true;
        }
    }

    void Chase()
    {
        agent.SetDestination(gamemanager.instance.player.transform.position);
    }

    void Attacking()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        agent.SetDestination(transform.position);

        if (!isShooting)
        {
            StartCoroutine(shoot());
        }

        if (playerInRange)
        {
            faceTarget();
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
        

        health -= damage;
        StartCoroutine(flashColor());

        if (health <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            if (gamemanager.instance.GetEnemyCount() <= 0)
            {
                gamemanager.instance.ShowWinMenu();
            }
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
