using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifeTime = 3f;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rb.velocity = transform.up * speed;
        Invoke(nameof(DestroyProjectile), lifeTime);

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        DestroyProjectile();
    }

    void DestroyProjectile() // TODO: Back to pool
    {
        Destroy(gameObject);
    }
}
