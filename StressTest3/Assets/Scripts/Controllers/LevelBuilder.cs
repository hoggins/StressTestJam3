using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
  public class LevelBuilder : MonoBehaviour
  {
    public List<GameObject> ChunkPrefabs;
    public float ChunkLength = 40;

    public void Awake()
    {
      for (int i = 0; i < 100; i++)
      {
        var prefab = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Count)];
        var go = Instantiate(prefab);
        go.transform.position = new Vector3(0, -2, i * ChunkLength);
      }
    }
  }
}