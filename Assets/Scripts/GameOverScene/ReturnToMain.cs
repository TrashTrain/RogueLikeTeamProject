using UnityEngine;
using UnityEngine.UI;

public class ReturnToMain : MonoBehaviour
{
    public void OnClickRTM()
    {
        SceneLoader.LoadScene("MainScene");
    }
}
