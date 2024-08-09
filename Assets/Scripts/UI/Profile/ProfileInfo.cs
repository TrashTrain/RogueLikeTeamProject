using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileInfo : MonoBehaviour
{
    public GameObject name;
    public GameObject atk;
    public GameObject def;
    public GameObject spd;

    private float originalATK = 10f;
    private float originalDEF = 10f;
    private float originalSPD = 5f;

    private float currentATK = 10f;
    private float currentDEF = 10f;
    private float currentSPD = 5f;

    void Start()
    {
        name.GetComponent<TextMeshProUGUI>().text = " Name : Player";
        atk.GetComponent<TextMeshProUGUI>().text = $" ATK :  {originalATK}";
        def.GetComponent<TextMeshProUGUI>().text = $" DEF :  {originalDEF}";
        spd.GetComponent<TextMeshProUGUI>().text = $" SPD :  {originalSPD}";
    }
    
    public void InitProfileInfo(float currentAtk, float currentDef, float currentSpd)
    {
        float atkDiff = currentAtk - originalATK;
        if (atkDiff > 0f)
        {
            atk.GetComponent<TextMeshProUGUI>().text = $" ATK :  {currentAtk}(+{atkDiff})";
        }
        else if (atkDiff < 0f)
        {
            atk.GetComponent<TextMeshProUGUI>().text = $" ATK :  {currentAtk}({atkDiff})";
        }
        else
        {
            atk.GetComponent<TextMeshProUGUI>().text = $" ATK :  {currentAtk}";
        }

        float defDiff = currentDef - originalDEF;
        if (defDiff > 0f)
        {
            def.GetComponent<TextMeshProUGUI>().text = $" DEF :  {currentDef}(+{defDiff})";
        }
        else if (defDiff < 0f)
        {
            def.GetComponent<TextMeshProUGUI>().text = $" DEF :  {currentDef}({defDiff})";
        }
        else
        {
            def.GetComponent<TextMeshProUGUI>().text = $" DEF :  {currentDef}";
        }

        float spdDiff = currentSpd - originalSPD;
        if (spdDiff > 0f)
        {
            spd.GetComponent<TextMeshProUGUI>().text = $" SPD :  {currentSpd}(+{spdDiff})";
        }
        else if (spdDiff < 0f)
        {
            spd.GetComponent<TextMeshProUGUI>().text = $" SPD :  {currentSpd}({spdDiff})";
        }
        else
        {
            spd.GetComponent<TextMeshProUGUI>().text = $" SPD :  {currentSpd}";
        }
    }
}
