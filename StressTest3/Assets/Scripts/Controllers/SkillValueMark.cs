using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
  public class SkillValueMark : MonoBehaviour
  {
    

    public void SetSkill(SkillKindId skill)
    {
      Color color;
      switch (skill)
      {
        case SkillKindId.Aoe:
          color = Color.green;
          break;
        case SkillKindId.Freeze:
          color = Color.blue;
          break;
        case SkillKindId.Heal:
          color = Color.red;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(skill), skill, null);
      }
      GetComponent<Image>().color = color;

    }
  }
}