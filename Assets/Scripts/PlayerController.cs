using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private InputHandler input;
    
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

    private Rigidbody2D _rb;
    public int Health { get; private set;}

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        input.OnShootPressed += Shoot;
    }

    private void OnDisable()
    {
        input.OnShootPressed -= Shoot;
    }

    private void Update()
    {
        
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
        //transform.Rotate(Vector3.forward, -input.RotationInput * rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        // TODO: cooldown
        
        AudioManager.Instance.PlaySFX(SFXType.Shoot);
        Projectile projectile = Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
        projectile.Initialize(shootSpeed);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        
        if(Health <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
