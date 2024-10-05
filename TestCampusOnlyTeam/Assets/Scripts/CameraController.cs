using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;
    [SerializeField] bool invertY;

    float rotx;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // invert y camera
        if (!invertY)
            rotx -= mouseY;
        else
            rotx += mouseX;

        // clamp the rotx on the x-axis
        rotx = Mathf.Clamp(rotx, lockVertMin, lockVertMax);

        // rotoate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(rotx, 0, 0);

        // rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}