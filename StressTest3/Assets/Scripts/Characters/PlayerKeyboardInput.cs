using System;
using UnityEngine;

namespace Characters
{
  public class PlayerKeyboardInput : MonoBehaviour
  {
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        Player.Instance.Attack(1);
      else if (Input.GetKeyDown(KeyCode.Alpha2))
        Player.Instance.Attack(2);
      else if (Input.GetKeyDown(KeyCode.Alpha3))
        Player.Instance.Attack(3);
      else if (Input.GetKeyDown(KeyCode.Alpha4))
        Player.Instance.Attack(4);
      else if (Input.GetKeyDown(KeyCode.Alpha5))
        Player.Instance.Attack(5);
    }
  }
}