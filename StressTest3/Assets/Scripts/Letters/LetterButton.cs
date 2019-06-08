using System;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Letters
{
  public class LetterButton : MonoBehaviour
  {
    public bool IsUsed { get; private set; }
    public Letter Letter { get; private set; }
    public event Action<LetterButton, Letter> OnLetter;

    public Text Text;
    public Button Button;
    public Image Image;

    public void SetLetter(Letter letter)
    {
      Reset();
      Letter = letter;
      if (Letter.Value.HasValue)
        Text.text = Letter.Value.Value.ToString();
      else if (Letter.OrbColor.HasValue)
        Image.color = Orb.GetColor(Letter.OrbColor.Value);
    }

    private void Reset()
    {
      Text.text = String.Empty;
      Image.color = Color.white;
    }

    public void LetterClick()
    {
      OnLetter?.Invoke(this, Letter);
    }

    public void SetUsed(bool isUsed)
    {
      IsUsed = isUsed;
//      gameObject.SetActive(!isUsed);
      Button.interactable = !isUsed;
    }
  }
}