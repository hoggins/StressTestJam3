using System;
using UnityEngine;
using UnityEngine.UI;

namespace Letters
{
  public class LetterButton : MonoBehaviour
  {
    public bool IsUsed { get; set; }
    public Text Text;
    public Button Button;
    private Letter _letter;
    public event Action<LetterButton, Letter> OnLetter;


    public void SetLetter(Letter letter)
    {
      _letter = letter;
      if (_letter.Value.HasValue)
        Text.text = _letter.Value.Value.ToString();
    }

    public void LetterClick()
    {
      OnLetter?.Invoke(this, _letter);
    }

    public void SetUsed(bool isUsed)
    {
      IsUsed = isUsed;
//      gameObject.SetActive(!isUsed);
      Button.interactable = !isUsed;
    }
  }
}