using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

//using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class PlayerController : MonoBehaviour, EnemyDamage
{
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] int sprintMod;

    [SerializeField] Camera cam;
    [SerializeField] CharacterController controller;

    [SerializeField] GameObject currentWeapon;
    [SerializeField] GameObject handgun;
    [SerializeField] GameObject shotgun;
    [SerializeField] GameObject submachinegun;
    Vector3 movementDir;
    int jumpCount;
    int HPOrig;
    Vector3 playerVelocity;
    bool isShooting;
    bool continuousFire;
    bool swapping;
    //GameObject pe;

    // Start is called before the first frame update
    void Start()
    {
        continuousFire = false;
        HPOrig = health;
        updatePlayerUI();
        spawnPlayer();

    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gamemanager.instance.playerSpawnPOS.transform.position;
        controller.enabled = true;
        health = HPOrig;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        movementDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(movementDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetButton("Fire1") &&!gamemanager.instance.isPaused&&!swapping)
        {
            if (currentWeapon != null)
            {
                IWeapon weapon = currentWeapon.GetComponent<IWeapon>();
                weapon.Fire(continuousFire);
                continuousFire = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            continuousFire = false;
        }

        if (Input.GetButtonDown("Reload"))
        {
            if (!continuousFire)
            {

                if (currentWeapon != null)
                {
                    IWeapon weapon = currentWeapon.GetComponent<IWeapon>();
                    weapon.Reload();
                }
            }
        }

        if (Input.GetButtonDown("Weapon1"))
        {
            swapWeapons(handgun);
        }

        if (Input.GetButtonDown("Weapon2"))
        {
            swapWeapons(shotgun);
        }
        if (Input.GetButtonDown("Weapon3"))
        {
            swapWeapons(submachinegun);
        }

    }

    void swapWeapons(GameObject weapon)
    {
        if(!continuousFire && !swapping)
        {
            if(currentWeapon != weapon)
            {
                StartCoroutine(doWeaponSwap(currentWeapon, weapon));
            }
        }
    }

    IEnumerator doWeaponSwap(GameObject inweapon, GameObject outweapon)
    {
        swapping = true;
        currentWeapon.SetActive(false);
        currentWeapon = outweapon;
        currentWeapon.SetActive(true);
        IWeapon weapon = currentWeapon.GetComponent<IWeapon>();
        if (weapon != null)
        {
            // Assuming your IWeapon interface or class has access to `clipCurrent` and `ammo`
            HitScanWeapon hitScanWeapon = currentWeapon.GetComponent<HitScanWeapon>();
            if (hitScanWeapon != null)
            {
                gamemanager.instance.updateAmmoUI(hitScanWeapon.clipCurrent, hitScanWeapon.ammo);
            }
        }
        yield return new WaitForSeconds(1);
        swapping = false;
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());

        if (health <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    IEnumerator damageFlash()
    {
        gamemanager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageScreen.SetActive(false);
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHpBar.fillAmount = (float)health / HPOrig;

    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    public void getBuffStats(BuffPickUps buff)
    {
        if (buff.HealAmount > 0)
        {
            health += buff.HealAmount;
            
            if (health > HPOrig)
            {
                health = HPOrig;
            }
            updatePlayerUI(); 
        }

       
        if (buff.SpeedMod > 0)
        {
            StartCoroutine(ApplySpeedBoost(buff.SpeedMod, buff.SpeedBoostDuration));
        }
        if (buff.ClipAmount > 0 && currentWeapon != null)
        {
            IWeapon weapon = currentWeapon.GetComponent<IWeapon>();
            if (weapon != null)
            {
                HitScanWeapon hitScanWeapon = currentWeapon.GetComponent<HitScanWeapon>();
                if (hitScanWeapon != null)
                {
                    hitScanWeapon.IncreaseClips(buff.ClipAmount);
                    gamemanager.instance.updateAmmoUI(hitScanWeapon.clipCurrent, hitScanWeapon.ammo);
                }
            }
        }
    }
    IEnumerator ApplySpeedBoost(float speedMultiplier, float duration)
    {
        
        speed *= speedMultiplier;

        
        yield return new WaitForSeconds(duration);

        
        speed /= speedMultiplier;
    }
}
