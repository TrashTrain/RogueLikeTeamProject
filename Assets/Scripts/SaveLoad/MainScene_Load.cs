using UnityEngine;

public class MainScene_Load : MonoBehaviour
{
    public void OnClickLoad()
    {
        UIManager.instance.GetComponentInChildren<LoadGameUI>().OpenLoadPanel();
    }
}
