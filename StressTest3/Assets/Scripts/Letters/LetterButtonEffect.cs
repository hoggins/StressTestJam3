using System;
using System.Collections;
using UnityEngine;

namespace Letters
{
  public class LetterButtonEffect : MonoBehaviour
  {
    private LetterButton _button;
    private float _endTime;
    private float _duration;
    private float _startTime;
    
    public void Setup(KeyboardEffectKindId effect, float duration)
    {
      Effect = effect;
      _button = GetComponent<LetterButton>();
      _duration = duration;
      _startTime = Time.time;

      StartCoroutine(LerpBlur());
      return;
      
      switch (effect)
      {
        case KeyboardEffectKindId.Blur:
          StartCoroutine(LerpBlur());
          break;
        case KeyboardEffectKindId.Flip:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
      }
      
    }

    public KeyboardEffectKindId Effect { get; set; }

    private IEnumerator LerpBlur()
    {
      float elapsed;
      var halfCycle = _duration /2;
      float t;
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / halfCycle;
        _button.Text.fontSize = (int) Mathf.Lerp(80, 5, t);
        _button.Text.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(20, 20, 1), t);
        _button.Text.color = Color.Lerp(new Color(1,1, 0, 1), new Color(1,1, 0, 0.3f), t);
        yield return null;
      } while (elapsed < halfCycle);

      _startTime = Time.time;
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / halfCycle;
        _button.Text.fontSize = (int) Mathf.Lerp(5, 80, t);
        _button.Text.transform.localScale = Vector3.Lerp(new Vector3(20, 20, 1), Vector3.one, t);
        _button.Text.color = Color.Lerp(new Color(1,1, 0, 0.3f), new Color(1,1, 0, 1), t);
        yield return null;
      } while (elapsed < halfCycle);
      
      Destroy(this);
    }

    private void OnDestroy()
    {
      _button.Text.fontSize = 80;
      _button.Text.transform.localScale = Vector3.one;
      
    }
  }
}