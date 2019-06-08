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
    
    /*public static Dictionary<SkillKindId, List<double>> SkillPower = new Dictionary<SkillKindId, List<double>>
    {
      [SkillKindId.Aoe] = 1,
      [SkillKindId.Attack] = 1,
      
    };*/
  }
}