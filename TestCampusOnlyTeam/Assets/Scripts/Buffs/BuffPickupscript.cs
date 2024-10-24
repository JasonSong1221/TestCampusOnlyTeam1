using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickupscript : MonoBehaviour
{
    [SerializeField] BuffPickUps buff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.playerScript.getBuffStats(buff);
            Destroy(gameObject);
        }
    }
}
