using System;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject RedEffect;
    public GameObject GreenEffect;
    public GameObject BlueEffect;

    public GameObject OrbPrefab;
    public float OrbDamageMultiplier = 2;

    public Animator Animator;



    private float _freezeDuration;

    private void Start()
    {
      var renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
      Enemies.Add(this);
      _currentPhase = _bossPhases.FirstOrDefault();
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
        _speedMod = Mathf.Lerp(_speedMod, 1f, Time.deltaTime * 2f);

        if (distance > StopDistance || _speedMod < 0)
          transform.position += Preset.Speed * Time.deltaTime * direction*_speedMod;
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



    [Serializable]
    public class BossPhase
    {
      public int LettersCount;
      public int HitsCount;
    }

    [Header("Boss")] public List<BossPhase> _bossPhases;

    public BossPhase _currentPhase;
    public int _currentBossPhase;
    public int BossDamageTakenTimes;

    private float _speedMod = 1f;

    public void TryTakeBossDamage(int lettersCount)
    {
      if (lettersCount >= _currentPhase.LettersCount)
      {

        _speedMod = -60f;

        BossDamageTakenTimes++;
        if (BossDamageTakenTimes > _currentPhase.HitsCount)
        {
          _currentBossPhase++;
          if (_currentBossPhase >= _bossPhases.Count)
          {
            DoDie();
          }
          else
          {
            _currentPhase = _bossPhases[_currentBossPhase];
          }
        }
      }
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
        DoDie();
      }
    }

    private void DoDie()
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
        case GameController.SpawnKind.Boss:
          AudioController.Instance.PlayDeathBig();
          break;
      }

      Destroy(gameObject);
    }

    public void SetKind(GameController.SpawnKind kind, EnemyColorKind colorKind)
    {
      Preset = GameController.Instance.Presets.Find(p => p.Kind == kind);
      ColorKind = colorKind;
      SpawnKind = kind;
      Hp = Preset.Hp;

      if (RedEffect != null)
      {
        RedEffect.SetActive(false);
        GreenEffect.SetActive(false);
        BlueEffect.SetActive(false);
      }

      switch (colorKind)
      {
        case EnemyColorKind.Red:
          if(RedEffect)
          RedEffect.SetActive(true);
          SetColor(RedTexture);
          break;
        case EnemyColorKind.Green:
          if(RedEffect)
          GreenEffect.SetActive(true);
          SetColor(GreenTexture);
          break;
        case EnemyColorKind.Blue:
          if(RedEffect)
          BlueEffect.SetActive(true);
          SetColor(BlueTexture);
          break;
      }
    }

    private void SetColor(Texture texture)
    {
      var renderers = GetComponentsInChildren<Renderer>();

      var material = renderers.FirstOrDefault(r => !r.gameObject.name.Contains("RabbitLaser")).material;
      material = new Material(material);
      material.mainTexture = texture;

      foreach (var r in renderers)
      {
        if (r.gameObject.name.Contains("RabbitLaser"))
          continue;

        r.sharedMaterial = material;
      }
    }

    public void Freeze(float duration)
    {
      _freezeDuration = Mathf.Max(_freezeDuration, duration);

    }
  }
}