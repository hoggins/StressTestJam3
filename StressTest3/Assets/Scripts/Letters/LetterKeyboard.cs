using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Letters
{
  public class LetterController
  {
    public bool IsComplete => _nextCell == _letters.Count;
    
    private List<LetterCell> _letters;

    private int _nextCell;

    public LetterController(List<LetterCell> letters)
    {
      _letters = letters.OrderBy(l=>l.Index).ToList();
    }

    public void InputNext(Letter letter)
    {
      _letters[_nextCell++].SetLetter(letter);
    }

    public List<Letter> GetValue()
    {
      return _letters.Select(l => l.Letter).Where(l => l != null).ToList();
    }

    public void Reset()
    {
      foreach (var letter in _letters)
      {
        letter.SetLetter(null);
      }
    }
  }

public class LetterKeyboard : MonoBehaviour
  {
    public GameObject ButtonRoot;
    public GameObject LetterRoot;
    private List<LetterButton> _buttons;
    private LetterController _letters;

    public Action<List<Letter>> OnSubmit;

    private void Awake()
    {
      _buttons = ButtonRoot.GetComponentsInChildren<LetterButton>().ToList();
      _letters = new LetterController(LetterRoot.GetComponentsInChildren<LetterCell>().ToList());
      _letters.Reset();
      
      foreach (var button in _buttons)
      {
        button.OnLetter += ButtonOnLetter;
      }
    }

    private void Start()
    {
      RechargeButtons(true);
    }

    public void Submit()
    {
      OnSubmit?.Invoke(_letters.GetValue());
      _letters.Reset();
      RechargeButtons();
    }

    private void ButtonOnLetter(LetterButton sender, Letter letter)
    {
      if (_letters.IsComplete)
        return;
      sender.SetUsed(true);
      _letters.InputNext(letter);
    }

    private void RechargeButtons(bool isForce = false)
    {
      var targetButtons = isForce ? _buttons : _buttons.Where(b => b.IsUsed).ToList();
      var letters = LetterCore.PickLetters(targetButtons.Count);

      for (var i = 0; i < targetButtons.Count; i++)
      {
        var button = targetButtons[i];
        button.SetLetter(new Letter(letters[i]));
        button.SetUsed(false);
      }
    }
  }
}