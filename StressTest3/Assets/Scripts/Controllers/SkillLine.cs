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
      foreach (var btn in Buttons)
      {
        var buttonColors = btn.colors;
        buttonColors.disabledColor = buttonColors.normalColor;
        buttonColors.pressedColor = buttonColors.normalColor;
        btn.colors = buttonColors;
      }

    }

    private void Start()
    {
      SetupButton(Buttons[0], SkillKindId.Aoe);
      SetupButton(Buttons[1], SkillKindId.Freeze);
      SetupButton(Buttons[2], SkillKindId.Heal);

      UpdateSelected();

      
    }

    private void SetupButton(Button button, SkillKindId skill)
    {
      button.onClick.AddListener(() => DoSelect(skill));
      var text = button.GetComponentInChildren<Text>();
      text.text = GameBalance.SkillPower[skill][_wordIdx].ToString("0.##");
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
        var buttonColors = btn.colors;
        
        var color = index == btnIdx ? Color.white : buttonColors.disabledColor;
        
        
        buttonColors.normalColor = color;
        btn.colors = buttonColors;
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