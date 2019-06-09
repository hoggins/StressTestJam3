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

    public void SetLetter(Letter letter, bool onlyText = false)
    {
      if (!onlyText)
        Reset();
      
      Letter = letter;
      if (Letter.Value.HasValue)
      {
        Text.text = Letter.Value.Value.ToString();
        SetLocked(false);
      }
      else if (Letter.OrbColor.HasValue)
      {
        Text.text = String.Empty;
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
      //SetColor(Color.red);
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

      var time = (1 + cur.Length) * 5;
      foreach (var cFx in cur)
      {
        cFx.ResetDuration(time);
      }
      
      if (cur.Any(e=>e.Effect == effect))
        return;
      
      var fx = gameObject.AddComponent<LetterButtonEffect>();
      fx.Setup(effect, time);
    }
  }
}