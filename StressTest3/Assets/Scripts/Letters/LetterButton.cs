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
      {
        var color = Orb.GetColor(Letter.OrbColor.Value);
        SetColor(color);
      }
    }

    private void SetColor(Color color)
    {
      var buttonColors = Button.colors;
      buttonColors.normalColor = color;
      Button.colors = buttonColors;
    }

    private void Reset()
    {
      Text.text = String.Empty;
      SetColor(Color.red);
      SetLocked(false);
    }

    public void LetterClick()
    {
      OnLetter?.Invoke(this, Letter);
    }

    public void SetUsed(bool isUsed)
    {
      IsUsed = isUsed;
      gameObject.SetActive(!isUsed);
    }

    public void SetLocked(bool locked)
    {
      var color = locked ? Color.gray : Color.red;
      SetColor(color);
    }
  }
}