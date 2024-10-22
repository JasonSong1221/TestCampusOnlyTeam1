using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayerControl : MonoBehaviour
{
    [SerializeField] int tSpeed;

    Vector3 tmoveDir;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += tmoveDir * tSpeed * Time.deltaTime;
        
    }
}
