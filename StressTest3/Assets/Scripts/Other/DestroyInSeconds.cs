using System.Collections;
using UnityEngine;

namespace Other
{
  public class DestroyInSeconds : MonoBehaviour
  {

    public float Seconds = 10f;
    IEnumerator Start()
    {
      yield return new WaitForSeconds(Seconds);
      Destroy(gameObject);
    }
  }
}