using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{

    public GameObject[] Info;
    public MenuPanel menu;

    //private bool[] isButtonClick;
    List<bool> isButtonClick = new List<bool>();

    private int check = 0;
    private void Awake()
    {
        for (int i = 0; i < Info.Length-1; i++)
        {
            isButtonClick.Add(false);
        }
    }
    public void OnMenuButtonClick(int a)
    {
        
        if(check != a)
        {
            for (int i = 0; i < Info.Length-1; i++)
            {
                isButtonClick[i] = false;
                Info[i].SetActive(isButtonClick[i]);
            }
            check = a;
        }
        
        if (!isButtonClick[a])
        {
            //Debug.Log("false");
            isButtonClick[a] = true;
            Info[a].SetActive(isButtonClick[a]);
        }
        else
        {
            //Debug.Log("true");
            isButtonClick[a] = false;
            Info[a].SetActive(isButtonClick[a]);
        }

    }
    public void OnClickStartPage()
    {
        //Time.timeScale = 1;
        //Info[2].SetActive(false);
        //Pause.OnApplicationPause(false); 
        
        //시작 버튼 클릭 시 : 메뉴 패널 끄기 및 초기화 / 시간 재실행 / 메인 씬으로 이동
        menu.SetMenuPanel(false);
        SceneLoader.LoadScene("MainScene");
    }

    public void OnClickVillage()
    {
        //Time.timeScale = 1;
        //Info[2].SetActive(false);
        //Pause.OnApplicationPause(false);
        
        //마을 버튼 클릭 시 메뉴 패널 끄기 및 초기화 / 시간 재실행 / 마을 씬으로 이동
        menu.SetMenuPanel(false);
        SceneLoader.LoadScene("Town Map");
    }
    
    ////
    // 주의 : 메뉴 스크롤의 세이브 로드 버튼이 아님, 로드 패널 내의 3개의 버튼 클릭 시 실행
    ////
    public void OnClickSaveLoad()
    {
        //Info[2].SetActive(false);
        
        //로드 시 메뉴 패널을 끄기 및 초기화 / 시간 재실행
        menu.SetMenuPanel(false);
    }
}
