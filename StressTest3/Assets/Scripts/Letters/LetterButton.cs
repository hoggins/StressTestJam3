using System;
using System.Linq;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Letters
{
  public enum KeyboardEffectKindId
  {
    Blur,
    Flip,
    Shuffle,
  }

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
      
      var cur = gameObject.GetComponents<LetterButtonEffect>();
      foreach (var fx in cur)
      {
        Destroy(fx);
      }
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
      var color = locked ? Color.gray : new Color(0.8f,0.87f,0,1);
      SetColor(color);
    }

    public void SetEffect(KeyboardEffectKindId effect)
    {
      if (!gameObject.activeSelf)
        return;
      var cur = gameObject.GetComponents<LetterButtonEffect>();
      if (cur.Any(e=>e.Effect == effect))
        return;
      
      var fx = gameObject.AddComponent<LetterButtonEffect>();
      fx.Setup(effect, 5);
    }
  }
}