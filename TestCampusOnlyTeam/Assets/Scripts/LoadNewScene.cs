using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{


    public string sceneName;

    


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")&&gamemanager.instance.GetEnemyCount()<=0)
        {
            // Load the new scene
            SceneManager.LoadScene(sceneName);
        }
    }

}
