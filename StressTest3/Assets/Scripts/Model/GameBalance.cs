using System.Collections.Generic;

namespace Model
{
  public enum SkillKindId
  {
    Aoe = 1,
    Freeze = 2,
    Heal = 3,
  }

  public static class GameBalance
  {
    public const int MeleeCooldown = 10;

    public const float OrbDropChance = 0.25f;
//    public const float OrbDropChance = 1f;

    // index in List is word length
    public static Dictionary<SkillKindId, List<double>> SkillPower = new Dictionary<SkillKindId, List<double>>
    {
      [SkillKindId.Aoe] = new List<double>{0,0,0,5,10,15,20,20,20,20},
      [SkillKindId.Freeze] = new List<double>{0,0,0,1,2,3,4,4},
      [SkillKindId.Heal] = new List<double>{0,0,0,0.1,0.2,0.4,0.4,0.4},
    };

    // index is word length
    public static List<SkillKindId?> SkillLayout = new List<SkillKindId?>
    {
      null,
      null,
      null,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
    };
  }
}