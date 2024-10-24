using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Renderer model;

    Color colorOrig;

    void Start()
    {
        colorOrig = model.material.color;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gamemanager.instance.playerSpawnPOS.transform.position = transform.position;
            StartCoroutine(flashColor());
        }
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        model.material.color = colorOrig;
    }

}
