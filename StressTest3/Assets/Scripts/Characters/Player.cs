using System;
using Controllers;
using UnityEngine;

namespace Characters
{
  public class Player : MonoBehaviour
  {
    public static Player Instance;
    public float MaxHp;
    public float Hp;
    public float Speed = 3;

    public Transform[] FirePoints;
    public GameObject BulletPrefab;

    private float _speed = 0f;


    public float Damage = 10;

    private void Awake()
    {
      Instance = this;
      Hp = MaxHp;
    }

    private void Update()
    {
      var closestEnemy = GetClosestEnemy(out var distance);

      if (closestEnemy != null)
      {
        var direction = (closestEnemy.transform.position - transform.position).normalized;
        transform.rotation =Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime*10);
      }

      if (!GameController.Instance.IsWaveInProgress)
        _speed = Mathf.Lerp(_speed, Speed, Time.deltaTime*3);
      else
        _speed = Mathf.Lerp(_speed, 0f, Time.deltaTime);

      transform.position += new Vector3(0, 0, Time.deltaTime*_speed);
    }

    public void ShootClosestEnemy(int strength)
    {
      var closestEnemy = GetClosestEnemy(out var distance);
      if (closestEnemy != null)
      {
        CameraController.Instance.Shaker.ShakeOnce(1.85f, 2f, 0f, 0.45f, new Vector3(), new Vector3(1,1,1));
        ShootEnemy(strength, closestEnemy);
      }
    }

    public void ShootAll(float strength)
    {
      foreach (var enemy in Enemy.Enemies)
      {
        ShootEnemy(strength, enemy);
      }

      if(Enemy.Enemies.Count > 0)
        CameraController.Instance.Shaker.ShakeOnce(1.85f+ Enemy.Enemies.Count /4f, 2f, 0f, 0.45f + Enemy.Enemies.Count / 5f, new Vector3(), new Vector3(1,1,1));
    }

    public void Heal(float percent)
    {
      Hp += MaxHp * percent;
      if (Hp > MaxHp)
        Hp = MaxHp;
    }

    public void FreezeEnemies(float duration)
    {
      foreach (var enemy in Enemy.Enemies)
      {
        enemy.Freeze(duration);
      }
    }


    public float MeleeDamage = 10f;
    public float MeleeRadius = 4f;
    public void MeleeAttack()
    {
      var colliders = Physics.OverlapSphere(transform.position, MeleeRadius);
      foreach (var c in colliders)
      {
        if (c.gameObject.CompareTag("Enemy"))
        {
          c.GetComponent<Enemy>().TakeDamage(MeleeDamage);
        }
      }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = new Color(1,0.25f,0.25f, 0.25f);
      Gizmos.DrawSphere(transform.position, MeleeRadius);
    }

    private int _currentFirePoint = 0;
    private void ShootEnemy(float strength, Enemy closestEnemy)
    {
      var go = GameObject.Instantiate(BulletPrefab);

      _currentFirePoint++;
      var firePoint = FirePoints[_currentFirePoint % FirePoints.Length];

      go.transform.position = firePoint.position;
      go.transform.localScale = new Vector3(1, 1, 1);

      var bullet = go.GetComponent<Bullet>();

      var direction = (closestEnemy.transform.position - firePoint.position).normalized;
      go.transform.rotation = Quaternion.LookRotation(direction);

      var damage = strength * Damage;
      bullet.Init(direction, 60, damage);
    }

    private Enemy GetClosestEnemy(out float minDistance)
    {
      Enemy closestEnemy = null;
      float closestDistance = 999999;
      foreach (var enemy in Enemy.Enemies)
      {
        var distance = Vector3.Distance(enemy.transform.position, transform.position);
        if (distance < closestDistance)
        {
          closestDistance = distance;
          closestEnemy = enemy;
        }
      }

      minDistance = closestDistance;
      return closestEnemy;
    }

    public void TakeDamage(float damage)
    {
      Hp -= damage;
      CameraController.Instance.Shaker.ShakeOnce(0.285f*damage, 2f, 0f, 0.85f, new Vector3(), new Vector3(1,1,1));
    }
  }
}