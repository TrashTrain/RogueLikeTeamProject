using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    public float playStartTime;
    public float playTime;
    public int currentSlot;

    //플레이어 일시 데이터 저장(임시)
    public float tempPlayerHp;
    
    public const string AutoSaveSlot = "autoSavefile.json";

    //Init for New Game
    public void InitPlayTime()
    {
        UIManager.instance.dialogSystem.npcObj.Clear();
        currentSlot = 0;
        playTime = 0;
        playStartTime = Time.time;
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playTime = 0;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public List<string> GetSaveFiles()
    {
        List<string> saveFiles = new List<string>();
        string path = Application.persistentDataPath;

        for (int i = 1; i <= 3; i++)
        {
            string filePath = Path.Combine(path, $"savefile_{i}.json");
            if (File.Exists(filePath))
            {
                saveFiles.Add(filePath);
            }
            else
            {
                saveFiles.Add(null);
            }
        }

        return saveFiles;
    }
    
    public void SaveGameData(GameData gameData, int slot)
    {
        string json = JsonUtility.ToJson(gameData);
        string path = Path.Combine(Application.persistentDataPath, $"savefile_{slot}.json");
        File.WriteAllText(path, json);
    }
    
    public void AutoSaveGameData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        string path = Path.Combine(Application.persistentDataPath, AutoSaveSlot);
        File.WriteAllText(path, json);
    }
    
    public void SaveGame(int slot)
    {
        UpdatePlayTime(currentSlot);
        
        PlayerController player = FindObjectOfType<PlayerController>();
        PlayerData playerData = new PlayerData(player.transform.position, player.maxhp, player.atk, player.def, player.speed, player.jumpPower);
        BasicPistol basicPistol = FindObjectOfType<PlayerController>().gameObject.transform.GetChild(0).GetComponent<BasicPistol>();
        PassiveSkillData passiveData = new PassiveSkillData(basicPistol.automaticBulletCnt, basicPistol.bulletSize);
        DialogData dialogData = new DialogData(UIManager.instance.dialogSystem.npcObj);

        GameData gameData = new GameData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, playerData, playTime, passiveData, dialogData);
        
        //string json = JsonUtility.ToJson(gameData);
        //File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        SaveGameData(gameData, slot);
    }
    
    public void AutoSaveGame()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        
        //메인메뉴 또는 플레이어가 없는 상황에서는 오토 세이브 파일을 삭제
        //오토 세이브는 씬 전환 시 일시적 데이터를 연동하기 위한 것.
        if (player == null)
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, AutoSaveSlot)))
            {
                File.Delete(Path.Combine(Application.persistentDataPath, AutoSaveSlot));
            }
            return;
        }
        
        Debug.Log("AutoSaveGame");
        
        PlayerData playerData = new PlayerData(player.transform.position, player.maxhp, player.atk, player.def, player.speed, player.jumpPower);
        BasicPistol basicPistol = player.gameObject.transform.GetChild(0).GetComponent<BasicPistol>();
        PassiveSkillData passiveData = new PassiveSkillData(basicPistol.automaticBulletCnt, basicPistol.bulletSize);
        DialogData dialogData = new DialogData(UIManager.instance.dialogSystem.npcObj);
        
        GameData gameData = new GameData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, playerData, playTime, passiveData, dialogData);
        
        AutoSaveGameData(gameData);
        
        //플레이어 현재 체력 저장(임시)
        tempPlayerHp = player.hp;
    }
    

    public void UpdatePlayTime(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath, $"savefile_{slot}.json");
        
        if (File.Exists(path))
        {
            playTime = LoadGameData(path).PlayTime;
        }
        else
        {
            playTime = 0;
        }
        playTime += Time.time - playStartTime;
    }

    public GameData LoadGameData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null;
    }

    public GameData AutoLoadGameData()
    {
        var path = Path.Combine(Application.persistentDataPath, AutoSaveSlot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null;
    }
    
    public async UniTask LoadGame(string filePath)
    {
        GameData gameData = LoadGameData(filePath);
        
        if (gameData == null)
        {
            Debug.Log("Load Fail : gameData is null");
            return;
        }
        
        //await SceneLoader.LoadScene(gameData.SceneName);
        await SceneManager.LoadSceneAsync(gameData.SceneName);
        ApplyPlayerData(gameData.PlayerData);
        ApplyPassiveData(gameData.PassiveSkillData);
        ApplyDialogData(gameData.DialogData);

        BGM.instance?.PlayBGM(gameData.SceneName);

        playStartTime = Time.time;
    }

    private void ApplyDialogData(DialogData dialogData)
    {
        if (dialogData == null) { Debug.Log("dialogDataNull"); return; }
        Dictionary<string, int> data = new ();
        for (int i = 0; i < dialogData.dialogName.Count; i++)
        {
            data.Add(dialogData.dialogName[i], dialogData.dialogIndex[i]);
        }
        UIManager.instance.dialogSystem.npcObj = data;
    }
    public void AutoLoadGame(int type)
    {
        GameData gameData = AutoLoadGameData();
        
        if (gameData == null)
        {
            Debug.Log("AutoLoadGame Fail");
            return;
        }
        
        Debug.Log("AutoLoadGame");
        ApplyPlayerDataForAuto(gameData.PlayerData, type);
        ApplyPassiveData(gameData.PassiveSkillData);
        ApplyDialogData(gameData.DialogData);
    }

    private void ApplyPassiveData(PassiveSkillData data)
    {
        BasicPistol passiveData = FindObjectOfType<BasicPistol>();
        if (passiveData == null)
        {
            Debug.Log("passiveDataNull");
            return;
        }
        passiveData.automaticBulletCnt = data.automaticBulletCnt;
        passiveData.bulletSize = data.bulletSize;
    }
    
    private void ApplyPlayerData(PlayerData playerData)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError($"{this.gameObject} : player is not found");
            return;
        }

        player.transform.position = playerData.Pos;
        player.maxhp = playerData.MaxHp;
        UIManager.instance.playerInfo.playerHpBar.InitPlayerHp(player.maxhp);
        player.atk = playerData.Atk;
        player.def = playerData.Def;
        player.speed = playerData.Speed;
        player.jumpPower = playerData.JumpPower;
    }
    
    private void ApplyPlayerDataForAuto(PlayerData playerData, int type)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError($"{this.gameObject} : player is not found");
            return;
        }
        
        //player.transform.position = playerData.Pos;
        player.maxhp = playerData.MaxHp;
        //UIManager.instance.playerInfo.playerHpBar.InitPlayerHp(player.maxhp);
        player.atk = playerData.Atk;
        player.def = playerData.Def;
        player.speed = playerData.Speed;
        player.jumpPower = playerData.JumpPower;

        //던전 내 빠른 로드
        if (type == 1)
        {
            //플레이어 현재 체력 연동(임시)
            player.hp = tempPlayerHp;
            //UIManager.instance.playerInfo.playerHpBar.SetHp(player.hp);
        }
        //로딩씬 있는 로드
        else if(type == 2)
        {
            //플레이어 체력을 최대체력으로 초기화
            player.hp = playerData.MaxHp;
            //UIManager.instance.playerInfo.playerHpBar.SetHp(player.hp);
        }
    }
}
