using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            BGM.instance.PlayBGM("GameOver");
            SceneManager.LoadScene("GameOver");
        }
        
        Destroy(other.gameObject);
    }
}
