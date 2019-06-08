using Characters;
using UnityEngine;

namespace Controllers
{
  public class CameraController : MonoBehaviour
  {
    public float Speed;
    public Vector3 Offset;

    public void LateUpdate()
    {
      if(Player.Instance == null)
        return;

      transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position + Offset, Time.deltaTime * Speed);

    }
  }
}