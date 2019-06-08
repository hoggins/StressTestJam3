using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
  public class Orb : MonoBehaviour
  {
    public EnemyColorKind Kind;

    public GameObject Red;
    public GameObject Green;
    public GameObject Blue;

    public float Accelaration = 5f;

    private float _speed = 0f;

    private void OnEnable()
    {
      Kind = (EnemyColorKind)Random.Range(0, 3);

      Red.gameObject.SetActive(false);
      Green.gameObject.SetActive(false);
      Blue.gameObject.SetActive(false);

      switch (Kind)
      {
        case EnemyColorKind.Red:
          Red.gameObject.SetActive(true);
          break;
        case EnemyColorKind.Green:
          Green.gameObject.SetActive(true);
          break;
        case EnemyColorKind.Blue:
          Blue.gameObject.SetActive(true);
          break;
      }
    }

    private void Update()
    {
      transform.position =
        Vector3.MoveTowards(transform.position, Player.Instance.transform.position, _speed * Time.deltaTime);
      _speed += Time.deltaTime*Accelaration;

      if (Vector3.Distance(transform.position, Player.Instance.transform.position) < 1f)
      {
        //todo: pick up orb to inventory
        Destroy(gameObject);
      }
    }
  }
}