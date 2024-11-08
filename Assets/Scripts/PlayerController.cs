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
    [SerializeField] private Transform shootPoint;
    [SerializeField] private WeaponBase currentWeapon;
    
    [FormerlySerializedAs("health")]
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] float invulnerableTime = 1f;


    bool invincible = false;
    private Rigidbody2D _rb;
    public int Health { get; private set;}

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
    }

    private void Start()
    {
        InitializeWeapon();
    }

    #region Enable/Disable
    private void OnEnable()
    {
        input.OnShootPressed += Shoot;
    }

    private void OnDisable()
    {
        input.OnShootPressed -= Shoot;
    }
    #endregion

    #region Movement
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
    #endregion

    #region Shooting
    void Shoot()
    {
        if(currentWeapon.CanShoot()) currentWeapon.Shoot();
    }
    
    void InitializeWeapon()
    {
        currentWeapon.SetShootPoint(shootPoint);
    }
    
    /*
    void SwitchWeapon(WeaponBase newWeapon)
    {
        Destroy(currentWeapon.gameObject);

        currentWeapon = newWeapon;
        InitializeWeapon();

    }
    */
    #endregion
    
    
    #region Health
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
    
    #endregion

    

    
}
