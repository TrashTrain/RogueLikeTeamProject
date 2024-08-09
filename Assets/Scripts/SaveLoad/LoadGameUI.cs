using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;

public class LoadGameUI : MonoBehaviour
{
    [SerializeField] private GameObject loadPanel;
    //[SerializeField] private LoadDoorMove loadDoorMove;
    [SerializeField] private Button[] saveButtons; // 3개의 버튼

    private bool isShow;
    private void Start()
    {
        LoadSaveFiles();
    }

    private void LoadSaveFiles()
    {
        List<string> saveFiles = DataManager.instance.GetSaveFiles();

        for (int i = 0; i < saveButtons.Length; i++)
        {
            if (i < saveFiles.Count)
            {
                string filePath = saveFiles[i];
                GameData gameData = DataManager.instance.LoadGameData(filePath);
                if (gameData != null)
                {
                    saveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = 
                        $"Save {i + 1}\n" +
                        $"{gameData.SceneName}\n" +
                        gameData.FormatPlaytime();
                        // $"MaxHP: {gameData.PlayerData.MaxHp}\n" +
                        // $"Atk: {gameData.PlayerData.Atk}\n" +
                        // $"Def: {gameData.PlayerData.Def}\n" +
                        // $"Speed: {gameData.PlayerData.Speed}\n" +
                        // $"JumpPower: {gameData.PlayerData.JumpPower}";
                    saveButtons[i].onClick.AddListener(() => OnSaveButtonClicked(filePath, i+1));
                    saveButtons[i].interactable = true; // 버튼 활성화
                }
            }
            else
            {
                saveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty Slot";
                saveButtons[i].interactable = false; // 버튼 비활성화
            }
        }
    }

    private async void OnSaveButtonClicked(string filePath, int slot)
    {
        Time.timeScale = 1;
        CloseLoadPanel();
        
        DataManager.instance.currentSlot = slot;
        await DataManager.instance.LoadGame(filePath);
        
    }

    public void OpenLoadPanel()
    {
        LoadSaveFiles();
        isShow = true;  
        LoadDoorMove.Instance.OnClickSetLoadPanel(loadPanel, isShow);
        //loadDoorMove.OnClickSetLoadPanel(loadPanel, isShow);
        
    }

    public void CloseLoadPanel()
    {
        isShow = false;
        //Debug.Log(LoadDoorMove.Instance);
        LoadDoorMove.Instance.OnClickSetLoadPanel(loadPanel, isShow);
        //loadDoorMove.OnClickSetLoadPanel(loadPanel, isShow);
    }
}