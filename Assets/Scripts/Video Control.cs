using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer video;

    void Start()
    {
        video.loopPointReached += CheckOver;
    }
    
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneLoader.LoadScene("Tutorial Map");
    }
}
