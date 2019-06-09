using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Letters;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
  public class GameController : MonoBehaviour
  {
    public static GameController Instance;

    public GameObject EnemyPrefab;
    public GameObject BigEnemyPrefab;
    public GameObject BossPrefab;
    public GameObject OrbRedUsePrefab;
    public GameObject OrbGreenUsePrefab;
    public GameObject OrbBlueUsePrefab;

    public float SpawnDistanceMin = 30;
    public float SpawnDistanceMax = 40;
    public float SpawnXDiff = 20;

    public float SpawnDifficulty = 1.8f;
    public float MaxSpawnPerWave = 10;

    public float CurrentWaveDifficultyLeft;

    public float TimeBetweenSpawnsMin = 2;
    public float TimeBetweenSpawnsMax = 5;

    public float TimeToWalkMin = 1;
    public float TimeToWalkMax = 3;

    public float DifficultyRising = 0.1f;
    public float MaxSpawnRising = 0.1f;

    public bool IsWaveInProgress => _isSpawnInProgres || Enemy.Enemies.Count != 0;
    private bool _isSpawnInProgres = false;
    private bool _spawnCoroutineStarted = false;

    public List<Enemy.EnemyPreset> Presets;

    public int BossSpawnWave = 15;
    public int CurrentWave = 0;

    public enum SpawnKind
    {
      Small,
      Big,
      Boss
    }

    public Dictionary<SpawnKind, float> SpawnCosts = new Dictionary<SpawnKind, float>()
    {
      {SpawnKind.Small, 0.75f},
      {SpawnKind.Big, 4.0f},
    };

    public float OrdDuration = 11;
    private float _orbDurationLeft;
    public EnemyColorKind ActiveOrb;

    void Awake()
    {
      Instance = this;
      ActiveOrb = EnemyColorKind.None;
      CurrentWave = 1;
    }

    private void Start()
    {
      if(!AudioController.Instance.Music.isPlaying)
        AudioController.Instance.PlayMusic();
    }

    void Update()
    {
      UpdateOrb();

      if (!IsWaveInProgress && !_spawnCoroutineStarted)
      {
        StartCoroutine(WaveCoroutine());
        SpawnDifficulty += DifficultyRising;
        MaxSpawnPerWave += MaxSpawnRising;
      }
    }

    private void UpdateOrb()
    {
      if(_orbDurationLeft <= 0)
        return;

      _orbDurationLeft -= Time.deltaTime;

      if (_orbDurationLeft <= 0)
        ActiveOrb = EnemyColorKind.None;
    }

    private IEnumerator WaveCoroutine()
    {
      _spawnCoroutineStarted = true;
      CurrentWaveDifficultyLeft = SpawnDifficulty;

      if (CurrentWave % BossSpawnWave == 0)
      {
        BattleHud.Instance.Boss();
        LetterController.Instance.Reset();
      }

      yield return new WaitForSeconds(Random.Range(TimeToWalkMin, TimeToWalkMax));

      if (CurrentWave % BossSpawnWave == 0)
      {
        _isSpawnInProgres = false;
        _spawnCoroutineStarted = false;

        Spawn(SpawnKind.Boss);

      CurrentWave++;
        yield break;
      }

      _isSpawnInProgres = true;

      while (CurrentWaveDifficultyLeft > 0)
      {
        if(!SpawnNextGroup(true))
          break;

        if(SpawnNextGroup(false))
          yield return new WaitForSeconds(Random.Range(TimeBetweenSpawnsMin, TimeBetweenSpawnsMax));
      }

      CurrentWave++;
      _isSpawnInProgres = false;
      _spawnCoroutineStarted = false;
    }



    private bool SpawnNextGroup(bool canSpawn)
    {



      var spawned = false;
      var waveSpawnAmount = Random.Range(1f, MaxSpawnPerWave);
      var groupDifficulty = Mathf.Min(CurrentWaveDifficultyLeft, waveSpawnAmount);

      if(canSpawn)
        CurrentWaveDifficultyLeft -= groupDifficulty;

      while (groupDifficulty > 0f)
      {
        var canSpawnItems = SpawnCosts.Where(i => i.Value < groupDifficulty).ToList();
        if (canSpawnItems.Count == 0)
          break;

        spawned = true;

        if (canSpawn)
        {
          var itemToSpawn = canSpawnItems[Random.Range(0, canSpawnItems.Count)];
          Spawn(itemToSpawn.Key);

          var score = ScoreController.Instance;
          var costMod = 1f;

          if (score.LettersWaseted > 0)
          {
            var coef = score.LettersUsed / score.LettersWaseted;
            costMod = coef;
          }

          costMod = Mathf.Clamp(costMod, 0.5f, 1f);

          groupDifficulty -= itemToSpawn.Value * costMod;
        }
        else
          break;
      }

      return spawned;
    }


    public void UseOrb(EnemyColorKind kind)
    {
      GameObject usePrefab = null;
      switch (kind)
      {
        case EnemyColorKind.Red:
          AudioController.Instance.PlayOrbRed();
          usePrefab = OrbRedUsePrefab;
          break;
        case EnemyColorKind.Green:
          AudioController.Instance.PlayOrbGreen();
          usePrefab = OrbGreenUsePrefab;
          break;
        case EnemyColorKind.Blue:
          AudioController.Instance.PlayOrbBlue();
          usePrefab = OrbBlueUsePrefab;
          break;
      }


      Instantiate(usePrefab, Player.Instance.transform.position + new Vector3(0, -2.39f, 20), Quaternion.identity);

      ActiveOrb = kind;
      _orbDurationLeft = OrdDuration;

      foreach (var enemy in Enemy.Enemies.ToList())
      {
        if (enemy.ColorKind != kind && enemy.SpawnKind != SpawnKind.Big && enemy.SpawnKind != SpawnKind.Boss)
          Destroy(enemy.gameObject);
        else
        {
          if (enemy.SpawnKind != SpawnKind.Big && enemy.SpawnKind != SpawnKind.Boss)
          {
            var go = CreateEnemyGo(SpawnKind.Big);
            var bigEnemy = go.GetComponent<Enemy>();
            bigEnemy.SetKind(SpawnKind.Big, enemy.ColorKind);
            bigEnemy.transform.position = enemy.transform.position;
            bigEnemy.transform.rotation = enemy.transform.rotation;

            Destroy(enemy.gameObject);
          }
          else
          {
            enemy.Hp = enemy.Preset.Hp;
          }
        }
      }
    }

    private void Spawn(SpawnKind kind)
    {
      var go = CreateEnemyGo(kind);

      go.transform.position = Player.Instance.transform.position
                              + new Vector3(Random.Range(-SpawnXDiff, SpawnXDiff),
                                0,
                                Random.Range(SpawnDistanceMin, SpawnDistanceMax));


      var enemy = go.GetComponent<Enemy>();

      var enemyKind = (EnemyColorKind)Random.Range(0, 3);
      enemy.SetKind(kind, enemyKind);
    }

    private GameObject CreateEnemyGo(SpawnKind kind)
    {
      GameObject go = null;
      switch (kind)
      {
        case SpawnKind.Small:
          go = GameObject.Instantiate(EnemyPrefab);
          break;
        case SpawnKind.Big:
          go = GameObject.Instantiate(BigEnemyPrefab);
          break;
        case SpawnKind.Boss:
          go = GameObject.Instantiate(BossPrefab);
          break;
      }

      return go;
    }
  }
}