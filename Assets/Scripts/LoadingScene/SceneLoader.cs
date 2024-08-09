using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    //로딩 화면 없는(동기적) 씬 전환 : 던전 내 씬 전환
    public static void LoadSceneFast(string sceneName)
    {
        DataManager.instance.AutoSaveGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 자동 로드 실행
        DataManager.instance.AutoLoadGame(1);
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 핸들러 제거
    }
    
    //로딩 화면 있는 씬 전환
    public static async UniTask LoadScene(string sceneName)
    {
        // 로딩 씬을 먼저 로드
        await LoadLoadingScene();

        // 로딩 씬의 스크립트를 통해 실제 씬을 비동기적으로 로드
        LoadingScreen loadingScreen = LoadingScreen.Instance;
        if (loadingScreen != null)
        {
            await loadingScreen.LoadSceneAsync(sceneName);
        }
    }

    private static async UniTask LoadLoadingScene()
    {
        //씬 로드 전 자동 세이브
        DataManager.instance.AutoSaveGame();
        
        // 비동기 씬 로드 시작
        var operation = SceneManager.LoadSceneAsync("LoadingScene");
        await UniTask.WaitUntil(() => operation.isDone);
    }
}