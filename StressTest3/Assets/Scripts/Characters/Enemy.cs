using System;
using System.Collections.Generic;
using Controllers;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
  public class Enemy : MonoBehaviour
  {
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
    public GameController.SpawnKind SpawnKind;
    private float _lastAttackTimer;

    public float StopDistance = 3;
    public static readonly List<Enemy> Enemies = new List<Enemy>();

    public List<Renderer> Renderers;

    public Texture RedTexture;
    public Texture GreenTexture;
    public Texture BlueTexture;

    public GameObject OrbPrefab;
    public float OrbDamageMultiplier = 2;

    public Animator Animator;



    private float _freezeDuration;

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
      if (_freezeDuration <= 0f)
      {
        var distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
        var direction = -(transform.position - Player.Instance.transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (distance > StopDistance)
          transform.position += Preset.Speed * Time.deltaTime * direction;
        else
        {
          DealDamageToPlayer();
        }
      }
      else
      {
        _freezeDuration -= Time.deltaTime;
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
      Animator.SetTrigger("Attack");
      Player.Instance.TakeDamage(Preset.Damage);
    }

    public void TakeDamage(float damage)
    {
      if (GameController.Instance.ActiveOrb != EnemyColorKind.None && GameController.Instance.ActiveOrb != ColorKind)
        damage *= OrbDamageMultiplier;

//      BattleLogController.Instance?.PushMessage($"Hit {name} for {damage} damage");

      Hp -= damage;

      Animator.SetTrigger("Hit");

      if (Hp <= 0)
      {
        if (Random.value <= GameBalance.OrbDropChance)
        {
          var go = GameObject.Instantiate(OrbPrefab);
          go.transform.position = transform.position;
        }

        switch (SpawnKind)
        {
          case GameController.SpawnKind.Small:
            AudioController.Instance.PlayDeathSmall();
            break;
          case GameController.SpawnKind.Big:
            AudioController.Instance.PlayDeathBig();
            break;
        }

        Destroy(gameObject);
      }
    }

    public void SetKind(GameController.SpawnKind kind, EnemyColorKind colorKind)
    {
      Preset = GameController.Instance.Presets.Find(p => p.Kind == kind);
      ColorKind = colorKind;
      SpawnKind = kind;
      Hp = Preset.Hp;

      switch (colorKind)
      {
        case EnemyColorKind.Red:
          SetColor(RedTexture);
          break;
        case EnemyColorKind.Green:
          SetColor(GreenTexture);
          break;
        case EnemyColorKind.Blue:
          SetColor(BlueTexture);
          break;
      }
    }

    private void SetColor(Texture texture)
    {
      foreach (var r in Renderers)
      {
        foreach (var material in r.materials)
        {
          material.mainTexture = texture;
//          material.SetTexture("_Main", texture);
        }
      }
    }

    public void Freeze(float duration)
    {
      _freezeDuration = Mathf.Max(_freezeDuration, duration);

    }
  }
}