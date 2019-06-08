using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
  public class MeleeButtonController : MonoBehaviour
  {
    public Text Text;
    public Button Button;

    private float? _activatedAt;

    public void OnMeleeButton()
    {
      _activatedAt = Time.time;
      Button.interactable = false;
      TryUpdateTimer();
    }

    private void TryUpdateTimer()
    {
      if (!_activatedAt.HasValue)
        return;
      
      var left = _activatedAt.Value + GameBalance.MeleeCooldown - Time.time;
      if (left <= 0)
      {
        Unlock();
        return;
      }

      Text.text = left.ToString("0.00");
    }

    private void Unlock()
    {
      _activatedAt = null;
      Text.text = "melee";
      Button.interactable = true;
    }

    private void Update()
    {
      TryUpdateTimer();
      
      
    }
  }
}