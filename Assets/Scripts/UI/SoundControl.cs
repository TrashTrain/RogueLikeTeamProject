using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [Header("soundCheck")]
    public GameObject[] soundCheckT;
    public GameObject[] soundCheckF;

    [Header("soundBarControl")]
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    [Header("MixerGroup")]
    public AudioMixerGroup bgm;

    [Header("AudioMixer")]
    public AudioMixer audioMixer;

    private string[] mixerNames = { "Master", "BGM", "SFX" };
    // Start is called before the first frame update
    void Awake()
    {
        //m_MusicMasterSlider.GetComponent<Slider>().value = 0.5f;
        //m_MusicBGMSlider.GetComponent<Slider>().value = 0.5f;        
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void OnVolume(float volume, string exposedName, bool _isVolume)
    {
        if (_isVolume)
        {
            audioMixer.SetFloat(exposedName, Mathf.Lerp(-80f, 0f, Mathf.Clamp01(volume)));
        }
        else
        {
            audioMixer.SetFloat(exposedName, Mathf.Lerp(0f, 0f, Mathf.Clamp01(volume)));

        }
    }
    public void SoundButtonClick(int select)
    {
        if (soundCheckT[select].activeSelf)
        {
            soundCheckT[select].SetActive(false);
            soundCheckF[select].SetActive(true);
            OnVolume(0, mixerNames[select], soundCheckF[select].activeSelf);
        }
        else
        {
            soundCheckT[select].SetActive(true);
            soundCheckF[select].SetActive(false);
            OnVolume(0, mixerNames[select], soundCheckF[select].activeSelf);
        }
    }
}
