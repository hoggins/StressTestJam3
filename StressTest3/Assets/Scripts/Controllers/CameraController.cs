using System;
using Characters;
using EZCameraShake;
using UnityEngine;

namespace Controllers
{
  public class CameraController : MonoBehaviour
  {
    public float Speed;
    public Vector3 Offset;
    [NonSerialized]
    public CameraShaker Shaker;

    public static CameraController Instance;
    public Vector3 Position;
    public Quaternion Rotation;

    private void Awake()
    {
      Shaker = new CameraShaker();
      Instance = this;
      Rotation = transform.rotation;
    }

    public void LateUpdate()
    {
      if(Player.Instance == null)
        return;

      Shaker.DoUpdate();
      Position = Vector3.Lerp(Position, Player.Instance.transform.position + Offset, Time.deltaTime * Speed);

      transform.position = Position + Shaker.PosAddShake;
      transform.rotation = Rotation*Quaternion.Euler(Shaker.RotAddShake);
    }
  }
}