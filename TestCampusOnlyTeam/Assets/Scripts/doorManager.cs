using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class doorManager : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float autoCloseTimer;
    private float timer;

    public float openAngle;
    public float closedAngle;


    public Vector3 doorDir;
    private bool isOpen = false;
    private bool playerInRange;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
#if false
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            isOpen = !isOpen;
        }

        if (!isOpen)
        {
            StartCoroutine(OpenDoor());

                timer += Time.deltaTime;
                if (timer >= autoCloseTimer)
                {
                    StartCoroutine(CloseDoor());
                    timer = 0f;
                }
        }
        else
        {
            StartCoroutine(CloseDoor());
        }

        if (Input.GetButtonDown("Open"))
        {
            OpenDoor();
            doorTimer();
        }
#endif
        if (Input.GetButtonDown("Interact"))
        {
            OpenDoor();
        }
    }

    private void MoveDoor(float angle)
    {
        // Set the doors angle based on whether it is open not
        targetAngle = angle;
        float currentAngle = Mathf.Round(transform.eulerAngles.y);

        // Check the y angle to against the target angle, if the target angle is not equal to the target angle
        if (Mathf.Abs(currentAngle - targetAngle) > 0.01f)
        {
            transform.Rotate(doorDir * speed * Mathf.Sign(targetAngle - currentAngle));
        }
        else
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = targetAngle;
            transform.eulerAngles = eulerAngles;
            doorDir = Vector3.zero;
        }
    }

    private void doorTimer()
    {
        isOpen = !isOpen;
        timer = 0f;
    }

    private void OpenDoor()
    {
        this.MoveDoor(openAngle);
    }

    private void CloseDoor()
    {
        this.MoveDoor(closedAngle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = !playerInRange;
            if (playerInRange)
            {
                //isOpen = !isOpen;
                isOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = !playerInRange;
            if (isOpen)
            {
                // isOpen = !isOpen;
                isOpen = false;
            }
            CloseDoor();
        }
    }

}
