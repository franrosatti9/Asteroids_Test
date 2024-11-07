using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private float minSize = .8f;
    [SerializeField] float maxSize = 4f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 6f;

    private float spawnTimer;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = spawnRate;
            SpawnRandomAsteroid();
        }
    }

    void SpawnRandomAsteroid()
    {
        Asteroid asteroid = Instantiate(asteroidPrefab);
        asteroid.transform.position = Random.insideUnitCircle.normalized * spawnRadius;
        
        Vector2 dir = (transform.position - asteroid.transform.position).normalized;
        
        asteroid.Initialize(dir, Random.Range(minSize, maxSize), Random.Range(minSpeed, maxSpeed));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
    }
}
