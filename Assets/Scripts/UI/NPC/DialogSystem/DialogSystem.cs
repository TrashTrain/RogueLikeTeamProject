using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public DialogSet[] dialogSets;

    public TextMeshProUGUI talkText;

    [Header("Select")]
    public RectTransform selectCursor;
    public GameObject selectPanel;
    private int talkIndex = 0;
    public bool isActiveSlot = true;

    private DialogSet curDialogSet;
    private DialogElement curDialog;

    public int nextDialogNum;

    [Header("NpcName")]
    public TextMeshProUGUI npcName;

    public Dictionary<string, int> npcObj = new ();

    private bool isActive = false;

    //타이핑 기능
    private bool isTyping = false;
    private bool skip = false;
    private float typingSpeed = 0.05f;
    
    private void Update()
    {
        if (selectPanel.activeSelf)
        {
            SelectPanelInput();
        }
        else
        {
            DialogInput();
        }
    }

    public void CallMethod(int methodNum = 0)
    {
        if (methodNum < 0)
            return;

        switch (methodNum)
        {
            case 0:
                NextSentence();
                //InActiveDialog();
                break;
            case 1:
                UIManager.instance.slotController.ShowSlotPanel();
                break;
            default:
                break;
        }

        selectPanel.SetActive(false);
    }
    public void ActiveDialog(int dialogSetIndex, string npcName)
    {
        if (isActive) return;
        if (dialogSetIndex < 0)
            return;

        isActive = true;
        talkIndex = 0;
        this.npcName.text = npcName;

        selectPanel.SetActive(false);

        bool idxCheck = false;
        for (int i = 0; i < dialogSets.Length; i++)
        {
            Debug.Log(dialogSets[i].IdxNum + "/" + dialogSetIndex);
            if (dialogSets[i].IdxNum == dialogSetIndex)
            {
                curDialogSet = dialogSets[i];
                idxCheck = true;
            }
        }
        gameObject.SetActive(idxCheck);
        if (gameObject.activeSelf)
            PlayerController.IsControllable = false;
        nextDialogNum = curDialogSet.nextIdx;

        if (!gameObject.activeSelf)
        {
            isActive = false;
            return;
        }
        NextSentence();

        if (npcObj.ContainsKey(npcName))
            npcObj[npcName] = nextDialogNum;
        else
            npcObj.Add(npcName, nextDialogNum);
    }

    public void InActiveDialog()
    {
        isActive = false;  

        PlayerController.IsControllable = true;
        gameObject.SetActive(false);
    }
    void NextSentence()
    {
        // 대사가 출력 중인데 재호출 시 모든 대사를 출력함
        if (isTyping)
        {
            skip = true;
            return;
        }
        
        //Debug.Log(talkIndex);
        if (talkIndex >= curDialogSet.dialogElements.Length)
        {
            InActiveDialog(); 
            return;
        }
        curDialog = curDialogSet.dialogElements[talkIndex];
        //talkText.text = curDialog.dialog;

        // 코루틴 호출하여 대사를 한글자씩 출력
        StartCoroutine(TypeSentence(curDialog.dialog));
        
        if (curDialog.selectYes >= 0)
            selectPanel.SetActive(true);

        talkIndex++;
        
    }

    void SelectPanelInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            if (isActiveSlot)
            {
                isActiveSlot = false;
            }
            else
            {
                isActiveSlot = true;
            }
            selectCursor.anchoredPosition = new Vector2(selectCursor.anchoredPosition.x, -selectCursor.anchoredPosition.y);
        }


        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isActiveSlot)
            {
                Debug.Log(curDialog.selectYes);
                CallMethod(curDialog.selectYes);
            }
            else
            {
                CallMethod(curDialog.selectNo);
            }
        }
    }

    private void DialogInput()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && !UIManager.instance.slotController.gameObject.activeSelf)
        {
            NextSentence();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.slotController.CloseSlotPanel();
        }
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        talkText.text = "";
        isTyping = true;
        skip = false;

        // 0.05초마다 한 글자씩 출력
        foreach (char letter in sentence.ToCharArray())
        {
            if (skip)
            {
                talkText.text = sentence;
                break;
            }
        
            talkText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skip = false;
    }

    public void SetTypeSpeed(float typeSpeed)
    {
        this.typingSpeed = typeSpeed;
    }

}

