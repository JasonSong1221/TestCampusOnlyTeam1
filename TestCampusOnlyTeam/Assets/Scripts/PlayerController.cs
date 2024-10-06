using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class PlayerController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] float fireRate;
    [SerializeField] float shootRange;
    [SerializeField] LayerMask ignore;
    [SerializeField] Camera cam;
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject gunImpactEffect;
    [SerializeField] int shootDMG;

    Vector3 movementDir;
    int jumpCount;
    Vector3 playerVelocity;
    bool isShooting;
    //GameObject pe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
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

        //if (Input.GetButton("Fire1") && !gameManager.instance.isPaused && !isShooting)
        //{
        //StartCoroutine(shoot());
        //}

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (!isShooting)
        {
            Animator ani = GetComponentInChildren<Animator>();
            if (ani != null)
            {
                ani.Play("Idle");
                
            }
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        GetComponent<AudioSource>().Play();

        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
        {
            ani.Play("Fire");

        }
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shootRange, ~ignore))
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
                dmg.takeDamage(shootDMG);
            }



        }


        yield return new WaitForSeconds(fireRate);
        ani.Play("Idle");

        isShooting = false;
    }
}
