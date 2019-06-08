using UnityEngine;

namespace Letters
{
  public class LetterBootstrap : MonoBehaviour
  {
    private void Awake()
    {
      LetterCore.Init();
    }
  }
}