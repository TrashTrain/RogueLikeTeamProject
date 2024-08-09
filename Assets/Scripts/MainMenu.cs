using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadCanvas;


    public void OnClickNewGame()
    {
        //SceneManager.LoadScene("Tutorial Map");
        SceneLoader.LoadScene("Opening Scene");
        DataManager.instance.InitPlayTime();
    }


    
    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    
}
