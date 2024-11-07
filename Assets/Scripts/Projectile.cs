using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Initialize(sp);
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

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Debug.Log("Damaged " + col.gameObject.name +". Health left: " + damageable.Health);
        }

        DestroyProjectile();
    }

    void DestroyProjectile() // TODO: Back to pool
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
