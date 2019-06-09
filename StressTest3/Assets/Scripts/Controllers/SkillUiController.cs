using System.Collections.Generic;
using System.Linq;
using Model;
using UI;
using UnityEngine;

namespace Controllers
{
  public class SkillUiController : MonoBehaviour
  {
    public List<SkillLine> Lines;
    public List<SkillValueMark> Marks;
    
    private void Awake()
    {
      Lines = GetComponentsInChildren<SkillLine>().ToList();
      Marks = GetComponentsInChildren<SkillValueMark>().ToList();

      var nextWIdx = 3;
      for (var index = 0; index < Lines.Count; index++)
      {
        var line = Lines[index];
        var mark = Marks[index];
        
        var selected = GameBalance.SkillLayout[nextWIdx].Value;
        mark.SetSkill(selected);
        line.Setup(nextWIdx, selected);
        line.OnSelect += LineOnOnSelect;
        nextWIdx++;
      }
    }

    private void LineOnOnSelect(int word, SkillKindId skillId)
    {
      Marks[word-3].SetSkill(skillId);
      GameBalance.SetSkill(word, skillId);
    }

    public void OnOk()
    {
      StartMenu.Instance.ToggleSkillUi();
    }
  }
}