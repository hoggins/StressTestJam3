using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
  public class SkillLine : MonoBehaviour
  {
    public List<Button> Buttons;
    private int _wordIdx;
    private SkillKindId _selected;

    public event Action<int, SkillKindId> OnSelect; 

    public void Setup(int wordIdx, SkillKindId selected)
    {
      _wordIdx = wordIdx;
      _selected = selected;
    }
    
    private void Awake()
    {
      Buttons = GetComponentsInChildren<Button>().ToList();
      
      Buttons[0].onClick.AddListener(()=> DoSelect(SkillKindId.Aoe));
      Buttons[1].onClick.AddListener(()=> DoSelect(SkillKindId.Freeze));
      Buttons[2].onClick.AddListener(()=> DoSelect(SkillKindId.Heal));

      UpdateSelected();
    }

    private void DoSelect(SkillKindId skillId)
    {
      OnSelect?.Invoke(_wordIdx, skillId);
      _selected = skillId;
      UpdateSelected();
    }

    private void UpdateSelected()
    {
      var btnIdx = ((int) _selected) - 1;
      for (var index = 0; index < Buttons.Count; index++)
      {
        var btn = Buttons[index];
        var color = index == btnIdx ? Color.red : Color.white;
        SetColor(btn, color);
      }
    }
    
    private void SetColor(Button btn, Color color)
    {
      var buttonColors = btn.colors;
      buttonColors.normalColor = color;
      btn.colors = buttonColors;
    }
  }
}