using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

namespace Controllers
{
  public class SkillUiController : MonoBehaviour
  {
    public List<SkillLine> Lines;
    
    private void Awake()
    {
      Lines = GetComponentsInChildren<SkillLine>().ToList();

      var nextWIdx = 3;
      foreach (var line in Lines)
      {
        line.Setup(nextWIdx, GameBalance.SkillLayout[nextWIdx].Value);
        line.OnSelect += LineOnOnSelect;
        nextWIdx++;
      }
    }

    private void LineOnOnSelect(int word, SkillKindId skillId)
    {
      GameBalance.SetSkill(word, skillId);
    }
  }
}