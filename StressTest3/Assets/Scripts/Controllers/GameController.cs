using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
  public class GameController : MonoBehaviour
  {
    public static GameController Instance;

    public GameObject EnemyPrefab;
    public GameObject BigEnemyPrefab;

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

    public enum SpawnKind
    {
      Small,
      Big
    }

    public Dictionary<SpawnKind, float> SpawnCosts = new Dictionary<SpawnKind, float>()
    {
      {SpawnKind.Small, 1.0f},
      {SpawnKind.Big, 4.0f},
    };

    public float OrdDuration = 11;
    private float _orbDurationLeft;
    public Enemy.EnemyColorKind ActiveOrb;

    void Awake()
    {
      Instance = this;
      ActiveOrb = Enemy.EnemyColorKind.None;
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
        ActiveOrb = Enemy.EnemyColorKind.None;
    }

    private IEnumerator WaveCoroutine()
    {
      _spawnCoroutineStarted = true;
      CurrentWaveDifficultyLeft = SpawnDifficulty;

      Debug.Log("Prepare wave");
      yield return new WaitForSeconds(Random.Range(TimeToWalkMin, TimeToWalkMax));
      Debug.Log("Do start");
      _isSpawnInProgres = true;

      while (CurrentWaveDifficultyLeft > 0)
      {
        Debug.Log("Try spawn next group");
        if(!SpawnNextGroup(true))
          break;

        Debug.Log("Spawn new group " + CurrentWaveDifficultyLeft);
        if(SpawnNextGroup(false))
          yield return new WaitForSeconds(Random.Range(TimeBetweenSpawnsMin, TimeBetweenSpawnsMax));
      }

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

          groupDifficulty -= itemToSpawn.Value;
        }
        else
          break;
      }

      return spawned;
    }

    public void UseOrb(Enemy.EnemyColorKind kind)
    {
      ActiveOrb = kind;
      _orbDurationLeft = OrdDuration;

      foreach (var enemy in Enemy.Enemies.ToList())
      {
        if (enemy.ColorKind == kind)
        {
          if (enemy.SpawnKind != SpawnKind.Big)
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

      var enemyKind = (Enemy.EnemyColorKind)Random.Range(0, 3);
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
      }

      return go;
    }
  }
}