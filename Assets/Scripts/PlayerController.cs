using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private InputHandler input;
    [SerializeField] private SpriteRenderer playerVisual;
    
    [Header("Movement")]
    [SerializeField] private float thrustForce = 3;
    [SerializeField] private float rotationSpeed = 30;

    [Header("Shooting")] 
    [SerializeField] private float shootSpeed;
    [SerializeField] private Projectile bulletPrefab; //TODO: Object pool
    [SerializeField] private Transform shootPoint;
    
    [FormerlySerializedAs("health")]
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] float invulnerableTime = 1f;
    
    ObjectPool<Projectile> _projectilePool;

    bool invincible = false;
    private Rigidbody2D _rb;
    public int Health { get; private set;}

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
    }

    void Start()
    {
        _projectilePool =
            new ObjectPool<Projectile>(CreateProjectile, GetProjectile, ReleaseProjecitle, DestroyProjectile);
    }

    #region Projectile Pool
    private void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    private void ReleaseProjecitle(Projectile projectile)
    {
        projectile.Reset();
        projectile.gameObject.SetActive(false);
    }

    private void GetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);

    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
        projectile.SetPool(_projectilePool);
        return projectile;
    }
    
    #endregion

    private void OnEnable()
    {
        input.OnShootPressed += Shoot;
    }

    private void OnDisable()
    {
        input.OnShootPressed -= Shoot;
    }
    
    private void FixedUpdate()
    {
        if (input.MovementInput != 0)
        {
            Thrust();
        }

        if (input.RotationInput != 0)
        {
            Rotate();
        }
    }

    void Thrust()
    {
        _rb.AddForce(transform.up * (thrustForce * input.MovementInput));
    }

    void Rotate()
    {
        _rb.AddTorque(-input.RotationInput * rotationSpeed);
    }

    void Shoot()
    {
        // TODO: cooldown
        
        AudioManager.Instance.PlaySFX(SFXType.Shoot);

        Projectile projectile = _projectilePool.Get();
        projectile.Initialize(shootSpeed);
    }

    public void TakeDamage(int damage)
    {
        if (invincible) return;
        
        Health -= damage;
        AudioManager.Instance.PlaySFXRandomPitch(SFXType.Death);
        
        if (Health <= 0)
        {
            Die();
            return;
        }
        
        StartCoroutine(InvincibilityFrames());
    }

    public void Die()
    {
        GameManager.Instance.FinishGame();
        Destroy(gameObject);
    }

    IEnumerator InvincibilityFrames()
    {
        invincible = true;
        playerVisual.color = new Color(1, 1, 1, 0.5f); 
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(invulnerableTime);
        playerVisual.color = new Color(1, 1, 1, 1);
        invincible = false;
        GetComponent<Collider2D>().isTrigger = false;
    }
}
