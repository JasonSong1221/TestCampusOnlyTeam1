using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{


    public string sceneName;

    // Replace "SceneName" with the name of the scene you want to load.


    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player (or any other specific tag you want to use)
        if (other.CompareTag("Player"))
        {
            // Load the new scene
            SceneManager.LoadScene(sceneName);
        }
    }

}
