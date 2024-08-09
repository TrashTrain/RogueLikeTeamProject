using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instance;
    
    private AudioSource audioSource;

    public AudioData[] soundResources;
    private Dictionary<string, AudioClip> BGMDB = new();
    
    private float originalPitch;    // 배경음악 원래 속도
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            audioSource = GetComponent<AudioSource>();
            // -----------
            originalPitch = audioSource.pitch;
            // -----------
            
            foreach (var soundResource in soundResources)
            {
                BGMDB.Add(soundResource.key, soundResource.Clip);
            }
            
            PlayBGM("MainScene");
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void PlayBGM(string key)
    {
        if (!BGMDB.ContainsKey(key))
        {
            Debug.LogError($"Unknown BGMDB key( )");
            StopBGM();
            return;
        }
        
        SetBGM(BGMDB[key]);
    }

    public void StopBGM()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    
    public void SetBGM(AudioClip bgm)
    {
        audioSource.Stop();
        audioSource.clip = bgm;
        audioSource.Play();
        audioSource.loop = true;
    }
    
    // 스피드 아이템 획득 시 배경음악 속도도 빠르게
    public void OnSpeedItemCollected(float duration)
    {
        StartCoroutine(SpeedItemEffectCoroutine(duration));
    }

    private IEnumerator SpeedItemEffectCoroutine(float duration)
    {
        audioSource.pitch = 1.5f; // 배경음악 속도 빠르게

        // 지속시간 동안 대기
        yield return new WaitForSeconds(duration);

        audioSource.pitch = originalPitch; // 배경음악 속도 원래대로
    }
    
    // 스피드 장애물 획득 시 배경음악 느리게
    public void OnSpeedObstacleCollected(float duration)
    {
        StartCoroutine(SpeedObstacleCoroutine(duration));
    }
    
    private IEnumerator SpeedObstacleCoroutine(float duration)
    {
        audioSource.pitch = 0.7f; // 배경음악 속도 빠르게
    
        // 지속시간 동안 대기
        yield return new WaitForSeconds(duration);
    
        audioSource.pitch = originalPitch; // 배경음악 속도 원래대로
    }

    public void SetOriginalPitch()
    {
        audioSource.pitch = originalPitch;
    }
}
