using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public AudioClip getItem;
    public AudioClip hpAtk;
    public AudioClip flyingEyeAttack;
    public AudioClip demonAttack;
    public AudioClip laserAttack;
    
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
