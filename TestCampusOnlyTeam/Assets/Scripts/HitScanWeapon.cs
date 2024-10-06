using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] float fireRate;
    [SerializeField] int damage;
    [SerializeField] float maxSpread;
    [SerializeField] float minSpread;
    [SerializeField] float spreadRate;
    [SerializeField] int range;
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject gunImpactEffect;
    [SerializeField] LayerMask ignore;
    [SerializeField] float damageFalloffPercent;
    [SerializeField] int startAmmo;
    [SerializeField] int maxAmmo;
    [SerializeField] int clipSize;

    bool isShooting;
    int continuousShots;
    int ammo;
    int clipCurrent;
    int maxClips;
    float spread;



    virtual public void Discard()
    {
        throw new System.NotImplementedException();
    }

    virtual public void Fire(bool continuous)
    {

        if(!isShooting && clipCurrent > 0)
        {
            if (continuous)
            {
                continuousShots++;
            }
            else
            {
                continuousShots = 0;
            }

            StartCoroutine(Shoot());
        }
    }

    virtual public void IncreaseAmmo(int amount)
    {
        if(ammo < maxAmmo)
        {
            ammo += amount;
            ammo = Mathf.Clamp(ammo, 0, maxAmmo);
        }
    }

    virtual public void Reload()
    {
        if(ammo > 0 && clipCurrent < clipSize)
        {
            int amtToAdd = Mathf.Clamp(clipSize - clipCurrent, 0, ammo);
            clipCurrent += amtToAdd;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (maxAmmo % clipSize != 0)
            throw new System.Exception("Clip Size needs to be a multiple of max ammo!");

        ammo = startAmmo;
        clipCurrent = Mathf.Clamp(clipCurrent, clipSize, ammo);
        maxClips = (maxAmmo / clipSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting)
        {
            Animator ani = GetComponentInChildren<Animator>();
            if (ani != null)
            {
                ani.Play("Idle");

            }
        }
    }

    IEnumerator Shoot()
    {


        isShooting = true;
        ammo--;
        clipCurrent--;

        GetComponent<AudioSource>().Play();

        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
        {
            ani.Play("Fire");

        }
        RaycastHit hit;
        Vector3 raycastPos = MainCamera.transform.position;
        Vector3 castDir = MainCamera.transform.forward;

        spread = spreadRate * continuousShots;
        spread = Mathf.Clamp(spread, minSpread, maxSpread);

        castDir.x += spread * UnityEngine.Random.Range(0.0f, 1.0f);
        castDir.y += spread * UnityEngine.Random.Range(0.0f, 1.0f);
        castDir.z += spread * UnityEngine.Random.Range(0.0f, 1.0f);


        if (Physics.Raycast(raycastPos, castDir, out hit, range, ~ignore))
        {
            Vector3 playerDir = Vector3.Normalize(hit.point - transform.position);
            Vector3 position = hit.point - (playerDir * 0.3f);

            Quaternion up = new Quaternion();

            up = Quaternion.LookRotation(Vector3.up);

            Instantiate(gunImpactEffect, position, up);
            //pe = Instantiate(gunImpactEffect, hit.point, new Quaternion());
            //GameObject.FindGameObjectWithTag("debug_cube").transform.position = hit.point;

            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }



        }


        yield return new WaitForSeconds(fireRate);
        //ani.Play("Idle");

        isShooting = false;
    }
}
