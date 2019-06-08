using System;
using UnityEngine;

namespace Characters
{
  public class Player : MonoBehaviour
  {
    public static Player Instance;
    public float MaxHp;
    public float Hp;

    public Transform FirePoint;
    public GameObject BulletPrefab;


    public int[] StrengthToDamage = new[]
    {
      1,
      10,
      50,
      100,
      200,
      500,
      1000,
      5000
    };

    private void Awake()
    {
      Instance = this;
      Hp = MaxHp;
    }

    private void Update()
    {

    }

    public void Attack(int strength)
    {
      var damage = strength < StrengthToDamage.Length
        ? StrengthToDamage[strength]
        : StrengthToDamage[StrengthToDamage.Length - 1];

      var closestEnemy = GetClosestEnemy(out var distance);

      if (closestEnemy != null)
      {
        var go = GameObject.Instantiate(BulletPrefab);
        go.transform.position = FirePoint.position;
        go.transform.rotation = FirePoint.rotation;
        go.transform.localScale = new Vector3(1,1,1);

        var bullet = go.GetComponent<Bullet>();

        var direction = (closestEnemy.transform.position - FirePoint.position).normalized;

        bullet.Init(direction, 5,  damage);
      }
    }

    private Enemy GetClosestEnemy(out float minDistance)
    {
      Enemy closestEnemy = null;
      float closestDistance = 999999;
      foreach (var enemy in Enemy.Enemies)
      {
        var distance = Vector3.Distance(enemy.transform.position, transform.position);
        if (distance < closestDistance)
        {
          closestDistance = distance;
          closestEnemy = enemy;
        }
      }

      minDistance = closestDistance;
      return closestEnemy;
    }

    public void TakeDamage(float damage)
    {
      Hp -= damage;
    }
  }
}