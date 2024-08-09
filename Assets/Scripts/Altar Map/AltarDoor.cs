using UnityEngine;

public class AltarDoor : MonoBehaviour
{

    public bool isPlayerInPortal = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isPlayerInPortal)
            {
                test();
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

    public void test()
    {
        SceneLoader.LoadScene("Town Map");
    }
}
