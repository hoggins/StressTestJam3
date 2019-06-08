using System;
using System.Collections.Generic;
using System.Numerics;
using Controllers;
using UnityEditor.Presets;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
  public class Enemy : MonoBehaviour
  {
    public enum EnemyColorKind
    {
      Red,
      Green,
      Blue
    }

    [Serializable]
    public class EnemyPreset
    {
      public float Hp;
      public float Speed;
      public float Damage;
      public float AttackCooldown;
      public GameController.SpawnKind Kind;
    }


    public float Hp = 10;

    [NonSerialized]
    public EnemyPreset Preset;

    public EnemyColorKind ColorKind;
    private float _lastAttackTimer;

    public float StopDistance = 3;
    public static readonly List<Enemy> Enemies = new List<Enemy>();

    void Awake()
    {
    }

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
        transform.position += Preset.Speed * Time.deltaTime * direction;
      else
      {
        DealDamageToPlayer();
      }

      PushCloseEnemies();
    }

    private void PushCloseEnemies()
    {
      foreach (var enemy in Enemies)
      {
        if (enemy == this)
          continue;

        var distance = Vector3.Distance(enemy.transform.position, transform.position);
        const float pushDistance = 1.5f;
        if (distance < pushDistance)
        {
          var dir = (transform.position - enemy.transform.position).normalized;
          transform.position += Time.deltaTime * (distance/pushDistance) * dir;
        }
      }
    }

    private void DealDamageToPlayer()
    {
      _lastAttackTimer -= Time.deltaTime;
      if(_lastAttackTimer >= 0)
        return;

      _lastAttackTimer = Preset.AttackCooldown;
      Player.Instance.TakeDamage(Preset.Damage);
    }

    public void TakeDamage(float damage)
    {
      Hp -= damage;

      if (Hp <= 0)
      {
        Destroy(gameObject);
      }
    }

    public void SetKind(GameController.SpawnKind kind, EnemyColorKind colorKind)
    {
      Preset = GameController.Instance.Presets.Find(p => p.Kind == kind);
      ColorKind = colorKind;
      Hp = Preset.Hp;
    }
  }
}