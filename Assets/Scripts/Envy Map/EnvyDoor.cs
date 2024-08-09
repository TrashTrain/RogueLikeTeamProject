using UnityEngine;

public abstract class EnvyDoor : MonoBehaviour
{
    public bool isPlayerInPortal = false;

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isPlayerInPortal)
            {
                test();
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isPlayerInPortal = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            isPlayerInPortal = false;
        }
    }

    protected abstract void test();
    // {
    //     SceneLoader.LoadScene("Town Map");
    // }
}
