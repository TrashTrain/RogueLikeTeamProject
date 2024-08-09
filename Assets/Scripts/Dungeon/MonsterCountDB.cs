using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCountDB
{
    //public List<int> countIndex = new();
    //public List<bool> countCheck = new();
    public Dictionary<int, bool> count = new ();

    public static MonsterCountDB Instance = new MonsterCountDB ();
    public void stageCheck(int stageIndex, bool check)
    {
        if (count.ContainsKey(stageIndex))
            return;
        count.Add (stageIndex, check);
    }

}

[System.Serializable]
public class MonsterCountData
{
    public List<int> stageIndex = new();
    public List<bool> stagecheck = new();

    public MonsterCountData(Dictionary<int, bool> stageData)
    {
        foreach (var data in stageData)
        {
            stageIndex.Add(data.Key);
            stagecheck.Add(data.Value);
        }
        
    }
}