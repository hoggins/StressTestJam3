using Characters;

namespace Letters
{
  public class Letter
  {
    public char? Value;
    public EnemyColorKind? OrbColor;

    public Letter(char v)
    {
      Value = v;
    }

    public Letter(EnemyColorKind kind)
    {
      OrbColor = kind;
    }
  }
}