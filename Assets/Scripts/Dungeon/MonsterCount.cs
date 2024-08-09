using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCount : MonoBehaviour
{
    public int mapIndex = 0;
    private int enemyCount = 0;
    private GameObject visibleWall;
    private void Start()
    {
        foreach (var data in MonsterCountDB.Instance.count)
        {
            if (data.Key == mapIndex)
            {
                if (!data.Value)
                {
                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

        }
        visibleWall = GameObject.Find("broken ground");
    }
    void Update()
    {
        if (visibleWall == null) return;
        EnemyCount();
    }

    public void EnemyCount()
    {
        enemyCount = transform.childCount;
        
        //Debug.Log("enemyCount : " + enemyCount);
        if (enemyCount == 0 && visibleWall.activeSelf)
        { 
            visibleWall.SetActive(false);
            MonsterCountDB.Instance.stageCheck(mapIndex, visibleWall.activeSelf);
        }
    }
}
