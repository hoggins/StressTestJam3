using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public Animator Animator;

    private float _speed = 0f;


    public float Damage = 10;

    private void Awake()
    {
      Instance = this;
      Hp = MaxHp;
    }

    IEnumerator Start()
    {
      while (true)
      {
        yield return new WaitForSeconds(Random.Range(15, 30));
        AudioController.Instance.PlayPhrase();
      }
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


      var passedDist = Time.deltaTime*_speed;
      ScoreController.Instance.TrackWalk(passedDist);
      transform.position += new Vector3(0, 0, passedDist);
    }

    public void ShootClosestEnemy(int strength)
    {
      var closestEnemy = GetClosestEnemy(out var distance);
      if (closestEnemy != null)
      {
        AudioController.Instance.PlayShoot();
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

      if (Enemy.Enemies.Count > 0)
      {
        AudioController.Instance.PlayMultishot();
        CameraController.Instance.Shaker.ShakeOnce(1.85f + Enemy.Enemies.Count / 4f, 2f, 0f,
          0.45f + Enemy.Enemies.Count / 5f, new Vector3(), new Vector3(1, 1, 1));
      }
    }

    public void Heal(float percent)
    {
      AudioController.Instance.PlayHeal();
      Hp += MaxHp * percent;
      if (Hp > MaxHp)
        Hp = MaxHp;
    }

    public void FreezeEnemies(float duration)
    {
      AudioController.Instance.PlayFreeze();
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
      int enemiesCount = 0;
      foreach (var c in colliders)
      {
        if (c.gameObject.CompareTag("Enemy"))
        {
          c.GetComponent<Enemy>().TakeDamage(MeleeDamage);
          enemiesCount++;
        }
      }

      AudioController.Instance.PlayHeroMelee();
      CameraController.Instance.Shaker.ShakeOnce(2.25f+ enemiesCount/4f, 2f, 0f, 0.65f + enemiesCount/ 5f, new Vector3(), new Vector3(1,1,1));
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

      if(_currentFirePoint%FirePoints.Length == 0)
        Animator.SetTrigger("Attack_0");

      if(_currentFirePoint%FirePoints.Length == 1)
        Animator.SetTrigger("Attack_1");

      go.transform.position = firePoint.position;
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

    private float _lastHitSoundPlayTime;
    public void TakeDamage(float damage)
    {
      if(Hp <= 0f)
        return;

      Hp -= damage;
      CameraController.Instance.Shaker.ShakeOnce(0.285f*damage, 2f, 0f, 0.85f, new Vector3(), new Vector3(1,1,1));

      if (_lastHitSoundPlayTime + 1f < Time.time)
      {
        AudioController.Instance.PlayHeroHit();
        _lastHitSoundPlayTime = Time.time;
        Animator.SetTrigger("Hit");
      }
    }
  }
}