using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Image = UnityEngine.UI.Image;

// 플레이어 프로필 스킬 정보
public class SkillController : MonoBehaviour
{
    public GameObject skillPrefab;

    public List<Skill> activeSkills = new List<Skill>();

    // 스킬 획득 시 플레이어 프로필에 추가
    // UIManager.instance.skillController.AddSkill(icon, "new skill");
    public void AddSkill(Sprite addedSkill, string skillName)
    {
        Debug.Log("in");
        GameObject newSkill = Instantiate(skillPrefab, transform);
        newSkill.GetComponentInChildren<Image>().sprite = addedSkill;
        
        Skill skill = new Skill
        {
            name = skillName,
            image = addedSkill
        };
        
        activeSkills.Add(skill);
    }

    public class Skill
    {
        public string name;
        public Sprite image;
    }
}
