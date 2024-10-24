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
    [SerializeField] int totalCasts;

    bool isShooting;
    int continuousShots;
    public int ammo;
    public int clipCurrent;
    int maxClips;
    float spread;
    bool isReloading;



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
        if (!isReloading && !isShooting)
        {
            StartCoroutine(reload());

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

        gamemanager.instance.updateAmmoUI(clipCurrent, ammo);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting && !isReloading)
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

        
        clipCurrent--;

        gamemanager.instance.updateAmmoUI(clipCurrent, ammo);

        GetComponent<AudioSource>().Play();

        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
        {
            ani.Play("Fire");
        }

        for (int i = 0; i < totalCasts; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = MainCamera.transform.position;
            Vector3 castDir = MainCamera.transform.forward;

            spread = spreadRate * continuousShots;
            spread = Mathf.Clamp(spread, minSpread, maxSpread);

            if (Physics.Raycast(raycastPos, castDir, out hit, range, ~ignore))
            {
                Vector3 playerDir = Vector3.Normalize(hit.point - transform.position);
                Vector3 position = hit.point - (playerDir * 0.003f);
                Quaternion up = Quaternion.LookRotation(Vector3.up);

                Instantiate(gunImpactEffect, position, up);

                Debug.Log(hit.collider.name);
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.takeDamage(damage, hit.point);
                }
            }
        }

        yield return new WaitForSeconds(fireRate);
        ani.Play("Idle");
        isShooting = false;
    }

    IEnumerator reload()
    {

        Animator ani = GetComponentInChildren<Animator>();

        if (ammo > 0 && clipCurrent < clipSize)
        {
            isReloading = true;

            if (ani != null)
            {
                ani.Play("Reload");
            }

            yield return new WaitForSeconds(0.5f);

            
            int bulletsNeeded = clipSize - clipCurrent;

            
            int bulletsToReload = Mathf.Min(bulletsNeeded, ammo);

           
            clipCurrent += bulletsToReload;
            ammo -= bulletsToReload;

            gamemanager.instance.updateAmmoUI(clipCurrent, ammo);
        }

        isReloading = false;
        ani.Play("Idle");
    }
    public void IncreaseClips(int clipCount)
    {
        
        int additionalAmmo = clipSize * clipCount;
        ammo += additionalAmmo;

       
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);
    }

}
