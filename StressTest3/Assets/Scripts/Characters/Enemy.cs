using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
  public class Enemy : MonoBehaviour
  {
    public enum EnemyKind
    {
      Red,
      Green,
      Blue
    }

    public float Hp = 10;
    public float Speed = 3;
    public float Damage = 1;
    public float AttackCooldown = 1;

    public EnemyKind Kind;
    private float _lastAttackTimer;

    public float StopDistance = 3;
    public static readonly List<Enemy> Enemies = new List<Enemy>();

    private void OnEnable()
    {
      Enemies.Add(this);
    }

    private void OnDisable()
    {
      Enemies.Remove(this);
    }

    private void Update()
    {
      var distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
      var direction = -(transform.position - Player.Instance.transform.position).normalized;

      if (distance > StopDistance)
        transform.position += Speed * Time.deltaTime * direction;
      else
      {
        DealDamageToPlayer();
      }
    }

    private void DealDamageToPlayer()
    {
      if(_lastAttackTimer >= 0)
        return;

      _lastAttackTimer = AttackCooldown;
      Player.Instance.TakeDamage(Damage);
    }

    public void TakeDamage(float damage)
    {
      Hp -= damage;

      if (Hp <= 0)
      {
        Destroy(gameObject);
      }
    }
  }
}