using System;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class BattleHud : MonoBehaviour
  {
    public Image PlayerHp;

    private void Update()
    {
      PlayerHp.fillAmount = Player.Instance.Hp/ Player.Instance.MaxHp;
    }
  }
}