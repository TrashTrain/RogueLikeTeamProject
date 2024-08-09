using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDoorMove : MonoBehaviour
{
    public static LoadDoorMove Instance;
    public Animator doorMove;

    private void Awake()
    {
        //Debug.Log("Instance");
        //Instance = this;
        
        
        if (Instance == null)
        {
            Instance = this;
            doorMove.updateMode = AnimatorUpdateMode.UnscaledTime;
            DontDestroyOnLoad(gameObject);
        }
        
        else
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator CloseDoor(GameObject setPanel, bool isShow)
    {
        yield return StartCoroutine(MoveDoorAni(true));

        if(isShow) setPanel.gameObject.SetActive(true);
        else setPanel.gameObject.SetActive(false);

        yield return StartCoroutine(MoveDoorAni(false));

    }

    private IEnumerator MoveDoorAni(bool doorClose)
    {
        if (doorClose)
        {
            doorMove.Play("DoorClose");
            yield return new WaitForSecondsRealtime(1f);
        }
        else
        {
            doorMove.Play("DoorOpen");
        }
        yield return null;
    }

    public void OnClickSetLoadPanel(GameObject setPanel, bool isShow)
    {
        //Debug.Log(setPanel);
        StartCoroutine(CloseDoor(setPanel, isShow));
    }

}
