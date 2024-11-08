using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IPoolObject<Projectile>
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;
    public ObjectPool<Projectile> Pool { get; private set; }

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public void Initialize(float speed)
    {
        _rb.velocity = transform.up * speed;
        Invoke(nameof(DestroyProjectile), lifeTime);

    }
    
    public void Reset()
    {
        _rb.velocity = Vector2.zero; 
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        Pool = pool;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        if (col.gameObject.TryGetComponent(out ScreenBoundaries boundaries) || // Don't destroy if colliding with DeathZone or Boundaries
            col.gameObject.TryGetComponent(out DeathZone deathZone)) return;
        
        DestroyProjectile();
        
    }
    
    public void ReleaseToPool()
    {
        CancelInvoke();
        Pool.Release(this);
    }

    void DestroyProjectile()
    {
        ReleaseToPool();
    }
}
