using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Letters;
using UnityEngine;

namespace Controllers
{
  public class InputController : MonoBehaviour
  {
    private void Start()
    {
      LetterKeyboard.Instance.OnSubmit += OnSubmit;
      LetterKeyboard.Instance.OnLetter += OnLetter;
      LetterKeyboard.Instance.OnMelee += () => Player.Instance.MeleeAttack();
    }

    private void OnLetter(Letter letter)
    {
      if (letter.OrbColor.HasValue)
        GameController.Instance.UseOrb(letter.OrbColor.Value);
      else
        Player.Instance.ShootClosestEnemy(1);
    }

    private void OnSubmit(List<Letter> obj)
    {
      var lettrs = obj.Select(l => l.Value.Value);
      var word = new string(lettrs.ToArray());
      var len = LetterCore.GetWordLength(word);
      SkillController.CastSkill(len);
    }
  }
}