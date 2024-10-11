using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{

    [SerializeField] enum DamageType { bullet, stationary, chaser }
    [SerializeField] DamageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        if (type == DamageType.bullet)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }

        else if (type == DamageType.chaser)
        {
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        EnemyDamage dmg = other.GetComponent<EnemyDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
            Destroy(gameObject);
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageType.chaser)
        {
            rb.velocity = (gamemanager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;//deltaTime must be update since it it changes
        }
    }
}




