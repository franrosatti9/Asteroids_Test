using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, IDamageable, IPoolObject<Asteroid>
{
    private Rigidbody2D _rb;
    private int scoreValue;
    public int Health { get; private set; }
    public ObjectPool<Asteroid> Pool { get; private set; }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector2 direction, float size, float speed)
    {
        Health = Mathf.Max(1, Mathf.RoundToInt(size));
        scoreValue = Health * 10;
        
        transform.localScale *= size;
        
        transform.up = direction;
        _rb.velocity = transform.up * speed;
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        
        AudioManager.Instance.PlaySFXRandomPitch(SFXType.DamageAsteroid);
        
        if(Health <= 0) Die();
    }

    public void Die()
    {
        //TODO: Deactivate WraparoundObject to avoid unnecessary calculations 
        
        AudioManager.Instance.PlaySFX(SFXType.DestroyAsteroid);
        GameManager.Instance.AddScore(scoreValue);
        Pool.Release(this);
    }
    
    public void Reset()
    {
        transform.localScale = Vector3.one;
        _rb.velocity = Vector2.zero;
        Health = 1;
    }

    public void SetPool(ObjectPool<Asteroid> pool)
    {
        Pool = pool;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(1);
            Pool.Release(this);
        }
    }
}
