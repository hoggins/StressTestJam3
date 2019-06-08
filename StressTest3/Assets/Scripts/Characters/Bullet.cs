using System;
using UnityEngine;

namespace Characters
{
  public class Bullet : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public float Damage;

    public bool IsDestroyed;

    public void Init(Vector3 direction, float speed, float damage)
    {
      Rigidbody.velocity = direction * speed;
      Damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
      if(IsDestroyed)
        return;
      
      if (other.gameObject.CompareTag("Enemy"))
      {
        var enemy = other.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(Damage);

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