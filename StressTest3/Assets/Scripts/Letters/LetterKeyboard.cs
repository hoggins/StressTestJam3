using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;

namespace Letters
{
  [RequireComponent(typeof(LetterBootstrap))]
  public class LetterKeyboard : MonoBehaviour
  {
    public static LetterKeyboard Instance { get; private set; }
    
    public GameObject ButtonRoot;
    public GameObject LetterRoot;
    private List<LetterButton> _buttons;
    private LetterController _letters;

    public Action<Letter> OnLetter;
    public Action<List<Letter>> OnSubmit;
    public Action OnMelee;

    private void Awake()
    {
      Instance = this;
      _buttons = ButtonRoot.GetComponentsInChildren<LetterButton>().ToList();
      _letters = new LetterController(LetterRoot.GetComponentsInChildren<LetterCell>().ToList());
      _letters.Reset();
      
      foreach (var button in _buttons)
      {
        button.OnLetter += ButtonOnLetter;
      }
    }

    private void OnDestroy()
    {
      Instance = null;
    }

    private void Start()
    {
      RechargeButtons(true);
    }

    public void PushOrb(EnemyColorKind kind)
    {
      var freeBtn = _buttons.FirstOrDefault(b => !b.IsUsed && !b.Letter.OrbColor.HasValue);
      if (freeBtn == null)
        return;
      freeBtn.SetLetter(new Letter(kind));
    }

    public void Submit()
    {
      OnSubmit?.Invoke(_letters.GetValue());
      _letters.Reset();
      RechargeButtons();
    }

    public void Melee()
    {
      OnMelee?.Invoke();
    }

    private void ButtonOnLetter(LetterButton sender, Letter letter)
    {
      sender.SetUsed(true);
      
      if (letter.OrbColor.HasValue)
      {
        OnLetter?.Invoke(letter);
        RechargeButtons();
        return;
      }
      else if (_letters.IsComplete)
        return;
      
      _letters.InputNext(letter);
      OnLetter?.Invoke(letter);
    }

    private void RechargeButtons(bool isForce = false)
    {
      var targetButtons = isForce ? _buttons : _buttons.Where(b => b.IsUsed).ToList();
      var letters = LetterCore.PickLetters(targetButtons.Count);

      for (var i = 0; i < targetButtons.Count; i++)
      {
        var button = targetButtons[i];
        if (letters.Count <= i)
          button.SetUsed(true);
        else
        {
          button.SetLetter(new Letter(letters[i]));
          button.SetUsed(false);
        }
      }
    }
  }
}