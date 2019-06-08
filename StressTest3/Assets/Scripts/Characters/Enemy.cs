using System;
using System.Collections.Generic;
using Controllers;
using UnityEditor.Presets;
using UnityEngine;

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

    public List<EnemyPreset> Presets;

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
      Preset = Presets.Find(p => p.Kind == kind);
      ColorKind = colorKind;
    }
  }
}