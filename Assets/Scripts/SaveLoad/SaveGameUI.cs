using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameUI : MonoBehaviour
{
    [SerializeField] private GameObject savePanel;
    [SerializeField] private Button[] saveButtons; // 3개의 버튼
    private bool isShow;
    
    private void Start()
    {
        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slot = i + 1;
            saveButtons[i].onClick.AddListener(() => OnSaveButtonClicked(slot));
        }

        UpdateSaveButtons();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (isShow)
        {
            CloseSavePanel();
        }
    }

    private void OnSaveButtonClicked(int slot)
    {
        DataManager.instance.SaveGame(slot);
        UpdateSaveButton(slot); // 저장 후 버튼 업데이트
    }

    private void UpdateSaveButtons()
    {
        List<string> saveFiles = DataManager.instance.GetSaveFiles();

        for (int i = 0; i < saveButtons.Length; i++)
        {
            int slot = i + 1;
            string filePath = Path.Combine(Application.persistentDataPath, $"savefile_{slot}.json");

            if (File.Exists(filePath))
            {
                GameData gameData = DataManager.instance.LoadGameData(filePath);

                if (gameData != null)
                {
                    saveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = 
                        $"Save {slot}\n" +
                        $"{gameData.SceneName}\n" +
                        gameData.FormatPlaytime();
                        // $"MaxHP: {gameData.PlayerData.MaxHp}\n" +
                        // $"Atk: {gameData.PlayerData.Atk}\n" +
                        // $"Def: {gameData.PlayerData.Def}\n" +
                        // $"Speed: {gameData.PlayerData.Speed}\n" +
                        // $"JumpPower: {gameData.PlayerData.JumpPower}";
                    
                    //saveButtons[i].interactable = true; // 버튼 활성화
                }
            }
            
            else
            {
                saveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty Slot";
                //saveButtons[i].interactable = false; // 버튼 비활성화
            }
        }
    }

    private void UpdateSaveButton(int slot)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"savefile_{slot}.json");

        if (File.Exists(filePath))
        {
            GameData gameData = DataManager.instance.LoadGameData(filePath);

            if (gameData != null)
            {
                saveButtons[slot - 1].GetComponentInChildren<TextMeshProUGUI>().text =
                    $"Save {slot}\n" +
                    $"{gameData.SceneName}\n" +
                    gameData.FormatPlaytime();
                // $"MaxHP: {gameData.PlayerData.MaxHp}\n" +
                // $"Atk: {gameData.PlayerData.Atk}\n" +
                // $"Def: {gameData.PlayerData.Def}\n" +
                // $"Speed: {gameData.PlayerData.Speed}\n" +
                // $"JumpPower: {gameData.PlayerData.JumpPower}";

                //saveButtons[slot - 1].interactable = true; // 버튼 활성화
            }
        }
        else
        {
            saveButtons[slot - 1].GetComponentInChildren<TextMeshProUGUI>().text = "Empty Slot";
            //saveButtons[slot - 1].interactable = false; // 버튼 비활성화
        }
    }

    public void OpenSavePanel()
    {
        if (isShow) return;
        isShow = true;  
        LoadDoorMove.Instance.OnClickSetLoadPanel(savePanel, isShow);
        
    }

    public void CloseSavePanel()
    {
        if (!isShow) return;
        isShow = false;
        LoadDoorMove.Instance.OnClickSetLoadPanel(savePanel, isShow);
    }
}
