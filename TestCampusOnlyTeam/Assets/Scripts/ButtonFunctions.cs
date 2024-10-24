using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
        gamemanager.instance.stateUnpause();
    }

    public void quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else   
        Application.Quit();
    #endif
    }

    public void respawn()
    {
        gamemanager.instance.playerScript.spawnPlayer();
        gamemanager.instance.stateUnpause();
    }
}
