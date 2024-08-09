using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDialogEvent : MonoBehaviour
{
    private string playerName = "주인공";
    private int playerIndex = 100;
    public int checkIndex;
    private string sceneName;

    void SetIndex()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Tutorial Map")
            playerIndex = 100;

        // ���� �Ҵ��ϴ� �޼����� ������ ���
        switch (sceneName)
        {
            default:
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            InitPlayerInfo();
            SetIndex();
            if(checkIndex == playerIndex)
                UIManager.instance.dialogSystem.ActiveDialog(playerIndex, playerName);
            //UIManager.instance.dialogSystem.charCurIndex = UIManager.instance.dialogSystem.nextDialogNum;
        }

        Destroy(gameObject);

    }

    private void InitPlayerInfo()
    {
        var curNPC = UIManager.instance.dialogSystem.npcObj;
        if (curNPC.ContainsKey(playerName))
        {
            playerIndex = curNPC[playerName];
        }
    }

}
