using System;
using Controllers;
using UnityEngine;

namespace Characters
{
  public class PlayerKeyboardInput : MonoBehaviour
  {
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        Player.Instance.ShootClosestEnemy(1);

      if(Input.GetKeyDown(KeyCode.A))
        Player.Instance.Heal(0.25f);
      else if(Input.GetKeyDown(KeyCode.S))
        Player.Instance.FreezeEnemies(3f);
      else if(Input.GetKeyDown(KeyCode.D))
        Player.Instance.ShootAll(3);
      else if(Input.GetKeyDown(KeyCode.F))
        Player.Instance.MeleeAttack();

      if (Input.GetKeyDown(KeyCode.Q))
        GameController.Instance.UseOrb(EnemyColorKind.Red);
      else if (Input.GetKeyDown(KeyCode.W))
        GameController.Instance.UseOrb(EnemyColorKind.Green);
      else if (Input.GetKeyDown(KeyCode.E))
        GameController.Instance.UseOrb(EnemyColorKind.Blue);
    }
  }
}