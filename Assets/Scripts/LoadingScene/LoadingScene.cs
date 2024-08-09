using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }
    public Slider loadingBar;
    public float minimumLoadingTime = 3f; // 최소 로딩 시간 설정

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        BGM.instance?.StopBGM();
        
        // 로딩 씬에서 슬라이더 찾기
        if (loadingBar == null)
        {
            loadingBar = FindObjectOfType<Slider>();
        }
    }
    
    public async UniTask LoadSceneAsync(string sceneName)
    {
        float startTime = Time.time;
        
        // 실제 씬을 비동기적으로 로드
        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
        
        if (loadOperation == null)
        {
            Debug.LogError("LoadingScene : loadOperation is null");
        }
        loadOperation.allowSceneActivation = false;
        
        
        
        // 씬 로드가 완료될 때까지 반복
        while (!loadOperation.isDone && loadOperation.progress <= 0.9f)
        {
            // 진행 상황 또는 경과 시간 중 최소값을 0에서 1 사이의 값으로 변환하여 슬라이더에 반영
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            float fakeProcess = Mathf.Clamp01((Time.time - startTime) / minimumLoadingTime);
            
            loadingBar.value = Mathf.Min(progress, fakeProcess);
            
            
            // 로딩이 완료되면 씬 전환을 허용
            if (loadOperation.progress >= 0.9f && Time.time - startTime >= minimumLoadingTime)
            {
                DataManager.instance.AutoLoadGame(2);
                loadingBar.value = 1f;
                loadOperation.allowSceneActivation = true;
                BGM.instance?.PlayBGM(sceneName);
            }

            await UniTask.Yield();
        }
    }
}