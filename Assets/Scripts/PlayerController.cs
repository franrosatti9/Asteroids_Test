using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputHandler input;
    
    [Header("Movement")]
    [SerializeField] private float thrustForce = 3;
    [SerializeField] private float rotationSpeed = 30;
    
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab; //TODO: Object pool
    [SerializeField] private Transform shootPoint;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        if (input.RotationInput != 0)
        {
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        if (input.MovementInput != 0)
        {
            Thrust();
        }
    }

    void Thrust()
    {
        _rb.AddForce(transform.up * (thrustForce * input.MovementInput));
    }

    void Rotate()
    {
        transform.Rotate(Vector3.forward, -input.RotationInput * rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
    }
}
