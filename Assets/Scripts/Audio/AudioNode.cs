using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNode : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private IEnumerator WaitSound()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        SoundManager.instance.SetNode(this);
    }
}
