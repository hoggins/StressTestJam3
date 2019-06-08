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
    private LetterKeyboard _keyboard;

    private void Awake()
    {
      LetterCore.Init();
      
      _keyboard = GetComponentInChildren<LetterKeyboard>();
      _keyboard.OnSubmit += OnSubmit;
      _keyboard.OnLetter += OnLetter;
      /*      if (Input.GetKeyDown(KeyCode.Alpha1))
        

      if(Input.GetKeyDown(KeyCode.A))
        Player.Instance.Heal(0.25f);
      else if(Input.GetKeyDown(KeyCode.S))
        Player.Instance.FreezeEnemies(3f);
      else if(Input.GetKeyDown(KeyCode.D))
        Player.Instance.ShootAll(3);
      else if(Input.GetKeyDown(KeyCode.F))
        Player.Instance.MeleeAttack();*/
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
      if (len == 3)
      {
        Player.Instance.ShootAll(1);
      }
      else if (len == 4)
      {
        Player.Instance.Heal(0.25f);
      }
      else if (len == 5)
      {
        Player.Instance.FreezeEnemies(5f);
      }
    }
  }
}