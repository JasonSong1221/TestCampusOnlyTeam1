using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorManager : MonoBehaviour
{
    public float speed;
    public float angle;

    public Vector3 doorDir;
    // Start is called before the first frame update
    void Start()
    {
        angle = transform.rotation.eulerAngles.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Round(transform.eulerAngles.y) != angle)
        {
            transform.Rotate(doorDir * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Open")) 
        {
            angle = 90;
            doorDir = Vector3.up;

        }

        if (Input.GetButtonDown("Close"))
        {
            angle = 0;
            doorDir = Vector3.up;
        }
    }
}
