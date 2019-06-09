using System;
using UnityEngine;

namespace Characters
{
  public class Bullet : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public float Damage;
    public int BossDamage;

    public bool IsDestroyed;

    public void Init(Vector3 direction, float speed, float damage, int bossDamage)
    {
      Rigidbody.velocity = direction * speed;
      Damage = damage;
      BossDamage = bossDamage;
      transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnCollisionEnter(Collision other)
    {
      if(IsDestroyed)
        return;

      if (other.gameObject.CompareTag("Enemy"))
      {
        var enemy = other.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(Damage);
        if(BossDamage > 1)
          enemy.TryTakeBossDamage(BossDamage);

        //todo: fx
        IsDestroyed = true;
        Destroy(gameObject);
      }
    }

    private void Update()
    {
    }
  }
}