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
    public float MaxSpawnPerWave = 50;

    public float CurrentWaveDifficultyLeft;

    public float TimeBetweenSpawnsMin = 2;
    public float TimeBetweenSpawnsMax = 5;

    public float TimeToWalkMin = 1;
    public float TimeToWalkMax = 3;

    public float DifficultyRising = 0.1f;

    public bool IsWaveInProgress => CurrentWaveDifficultyLeft > 0 || Enemy.Enemies.Count != 0;

    public List<Enemy.EnemyPreset> Presets;

    public enum SpawnKind
    {
      Small,
      Big
    }

    public Dictionary<SpawnKind, float> SpawnCosts = new Dictionary<SpawnKind, float>()
    {
      {SpawnKind.Small, 1.0f},
      {SpawnKind.Big, 5.0f},
    };

    void Awake()
    {
      Instance = this;
    }

    void Update()
    {
      if (!IsWaveInProgress)
      {
        StartCoroutine(WaveCoroutine());
        SpawnDifficulty += DifficultyRising;
      }
    }

    private IEnumerator WaveCoroutine()
    {
      CurrentWaveDifficultyLeft = SpawnDifficulty;
      Debug.Log("Prepare wave");
      yield return new WaitForSeconds(Random.Range(TimeToWalkMin, TimeToWalkMax));
      Debug.Log("Do start");

      while (CurrentWaveDifficultyLeft > 0)
      {
        Debug.Log("Try spawn next group");
        if(!SpawnNextGroup())
          break;

        Debug.Log("Spawn new group " + CurrentWaveDifficultyLeft);
        yield return new WaitForSeconds(Random.Range(TimeBetweenSpawnsMin, TimeBetweenSpawnsMax));
      }
    }



    private bool SpawnNextGroup()
    {
      var spawned = false;
      var waveSpawnAmount = Random.Range(1f, MaxSpawnPerWave);
      var groupDifficulty = Mathf.Min(CurrentWaveDifficultyLeft, waveSpawnAmount);

      CurrentWaveDifficultyLeft -= groupDifficulty;

      while (groupDifficulty > 0f)
      {
        var canSpawnItems = SpawnCosts.Where(i => i.Value < groupDifficulty).ToList();
        if (canSpawnItems.Count == 0)
          break;

        var itemToSpawn = canSpawnItems[Random.Range(0, canSpawnItems.Count)];
        Spawn(itemToSpawn.Key);

        groupDifficulty -= itemToSpawn.Value;
        spawned = true;
      }

      return spawned;
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