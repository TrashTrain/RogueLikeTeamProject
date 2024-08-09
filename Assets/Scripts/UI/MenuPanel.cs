using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    public GameObject menuScroll;

    private bool isMenuClick = false;
    public void OnMenuButtonClik()
    {
        Debug.Log(isMenuClick);
        
        if (!PlayerController.IsControllable)
        {
            return;
        }
        if (!isMenuClick)
        {
            SetMenuPanel(true);
            //Pause.OnApplicationPause(true);
            Time.timeScale = 0;
        }
        
    }
    private void Update()
    {
        if (!PlayerController.IsControllable) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuClick)
            {
                SetMenuPanel(false);
                //Pause.OnApplicationPause(false);
                Time.timeScale = 1;

            }
            else
            {
                if (SceneManager.GetActiveScene().name == "MainScene" ||
                    SceneManager.GetActiveScene().name == "GameOver" ||
                    SceneManager.GetActiveScene().name == "LoadingScene") return;
                SetMenuPanel(true);
                //Pause.OnApplicationPause(true);
                Time.timeScale = 0;
            }
            
            
        }
    }

    public void SetMenuPanel(bool click)
    {
        isMenuClick = click;
        menuScroll.SetActive(isMenuClick);
        Time.timeScale = (click) ? 0 : 1;
    }
}
