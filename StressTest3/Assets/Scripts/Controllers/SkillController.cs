using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Model;
using UnityEngine;

namespace Controllers
{
  public class SkillController {/*: MonoBehaviour
  {
    public SkillController Instance { get; private set; }

    private void Awake()
    {
      Instance = this;
    }

    private void OnDestroy()
    {
      Instance = null;
    }*/

    public static void CastSkill(int wordLen)
    {
      var skillKindId = GameBalance.SkillLayout.GetAtOrLast(wordLen);
      if (!skillKindId.HasValue)
        return;

      var power = GameBalance.SkillPower[skillKindId.Value].GetAtOrLast(wordLen);

      switch (skillKindId)
      {
        case SkillKindId.Aoe:
          Player.Instance.ShootAll((int)power);
          break;
        case SkillKindId.Freeze:
          Player.Instance.FreezeEnemies((float) power);
          break;
        case SkillKindId.Heal:
          Player.Instance.Heal((float) power);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public static class EnumExt
  {
    public static T GetAtOrLast<T>(this IList<T> list, int idx)
    {
      if (list.Count <= idx)
        return list.LastOrDefault();
      return list[idx];
    }
  }
  
}