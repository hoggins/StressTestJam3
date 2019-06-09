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

      
      switch (effect)
      {
        case KeyboardEffectKindId.Blur:
          StartCoroutine(LerpBlur());
          break;
        case KeyboardEffectKindId.Flip:
          StartCoroutine(LerpFlip());
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
      }
      
    }

    private IEnumerator LerpFlip()
    {
//      _button.Text.transform.localScale = new Vector3(-1,1,1);

      var originalRotation = _button.Text.transform.rotation;
      float t;
      float elapsed;
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /4);
        
        _button.Text.transform.rotation = Quaternion.Lerp(originalRotation ,Quaternion.Euler(0,0,179),t);
        yield return null;
      } while (elapsed < _duration /4);
      
      _startTime = Time.time;
      originalRotation = _button.Text.transform.rotation;
      
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /4);
        
        _button.Text.transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0,0,358) ,t);
        
        yield return null;
      } while (elapsed < _duration /4);
      
      _startTime = Time.time;
      originalRotation = _button.Text.transform.rotation;
      
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /4);
        
        _button.Text.transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0,0,150) ,t);
        
        yield return null;
      } while (elapsed < _duration /4);
      
      _startTime = Time.time;
      originalRotation = _button.Text.transform.rotation;
      
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /4);
        
        _button.Text.transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0,0,0) ,t);
        
        yield return null;
      } while (elapsed < _duration /4);
      
//      yield return new WaitForSeconds(_duration);
      Destroy(this);
    }

    public KeyboardEffectKindId Effect { get; set; }

    private IEnumerator LerpBlur()
    {
      float elapsed;
      float t;
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /2);
        _button.Text.fontSize = (int) Mathf.Lerp(80, 5, t);
        _button.Text.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(20, 20, 1), t);
        _button.Text.color = Color.Lerp(new Color(1,1, 0, 1), new Color(1,1, 0, 0.3f), t);
        yield return null;
      } while (elapsed < _duration /2);

      _startTime = Time.time;
      do
      {
        elapsed = Time.time - _startTime;
        t = elapsed / (_duration /2);
        _button.Text.fontSize = (int) Mathf.Lerp(5, 80, t);
        _button.Text.transform.localScale = Vector3.Lerp(new Vector3(20, 20, 1), Vector3.one, t);
        _button.Text.color = Color.Lerp(new Color(1,1, 0, 0.3f), new Color(1,1, 0, 1), t);
        yield return null;
      } while (elapsed < _duration /2);
      
      Destroy(this);
    }

    private void OnDestroy()
    {
      _button.Text.fontSize = 80;
      _button.Text.transform.localScale = Vector3.one;
      _button.Text.color = new Color(1, 1, 0, 1);
      _button.Text.transform.rotation = new Quaternion(0,0,0,0);
    }

    public void ResetDuration(int duration)
    {
      _duration = duration;
      _startTime = Time.time;
    }
  }
}