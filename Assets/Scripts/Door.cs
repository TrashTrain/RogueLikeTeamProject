using UnityEngine;

public class Door : MonoBehaviour
{
    public string nextSceneName;
    public bool isPlayerInPortal = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isPlayerInPortal)
            {
                SceneLoader.LoadScene(nextSceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isPlayerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isPlayerInPortal = false;
        }
    }
}