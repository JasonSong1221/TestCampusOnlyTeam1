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

        if (Input.GetButton("Fire1") && !swapping)
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
        gamemanager.instance.playerHPBar.fillAmount = (float)health / HPOrig;

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
}
