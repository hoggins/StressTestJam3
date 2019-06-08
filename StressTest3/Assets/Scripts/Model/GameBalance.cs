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
    
    public const float OrbDropChance = 0.05f;
    public const float OrbDropChance = 1f;
    
    // index in List is word length
    public static Dictionary<SkillKindId, List<double>> SkillPower = new Dictionary<SkillKindId, List<double>>
    {
      [SkillKindId.Aoe] = new List<double>{0,0,1,2,3,4,5,6,7},
      [SkillKindId.Freeze] = new List<double>{0,0,0.1,0.2,0.3,0.4,0.5},
      [SkillKindId.Heal] = new List<double>{0,0,0.2,0.4,0.6,0.8,1},
      
    };
    
    // index is word length
    public static List<SkillKindId?> SkillLayout = new List<SkillKindId?>
    {
      null,
      null,
      SkillKindId.Aoe,
      SkillKindId.Freeze,
      SkillKindId.Heal,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
      SkillKindId.Aoe,
    };
    
  }
}