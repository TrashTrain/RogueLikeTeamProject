using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySystem : MonoBehaviour
{
    public static int Priority(int[] ItemPerTable)
    {
        if (ItemPerTable.Length == 0)
        {
            return -1;

        }
        int number = 0;
        int sum = 0;
        for (int i = 0; i < ItemPerTable.Length; i++)
        {
            sum += ItemPerTable[i];
        }
        number = Random.Range(0, sum) + 1;

        sum = 0;
        for (int j = 0; j < ItemPerTable.Length; j++)
        {
            if (number <= ItemPerTable[j] + sum)
            {
                return j;
            }
            sum += ItemPerTable[j];
        }
        return -1;
    }
}
