using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    [Header("RandomSlotNPC")]
    public GameObject randomSlot;
    public RectTransform selectCursor;
    public GameObject selectImage;
    private bool isRandomSlot = true;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }
    void GenerateData()
    {
        // 배치물은 100번대, 대화가능한 npc 는 1000대부터
        talkData.Add(1000, new string[] { "레벨포인트 3을 소모해서 랜덤 효과를 얻을 수 있습니다.", "사용하시겠습니까?" });
        talkData.Add(100, new string[] { "별 볼일없는 쓸모없는 통이다." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
        {
            if(id == 1000)
            {
                selectImage.SetActive(true);
                return talkData[id][talkIndex - 1];
            }
            return null;
        }
        else
            return talkData[id][talkIndex];
    }
    private void Update()
    {
  
    }

    void GetRandomSlot()
    {
        randomSlot.SetActive(true);
    }
}
