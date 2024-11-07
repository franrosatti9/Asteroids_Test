using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;



public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private float minSize = .8f;
    [SerializeField] float maxSize = 4f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 6f;

    [SerializeField] private int maxAsteroidsOnScreen = 10;
    
    ObjectPool<Asteroid> _pool;

    private float spawnTimer;
    
    void Start()
    {
        _pool = new ObjectPool<Asteroid>(CreateAsteroid, GetAsteroid, ReturnToPool, DestroyAsteroid);
    }

    #region Pool
    
    private void DestroyAsteroid(Asteroid asteroid)
    {
        Destroy(asteroid.gameObject);
    }

    private void ReturnToPool(Asteroid asteroid)
    {
        asteroid.Reset();
        asteroid.gameObject.SetActive(false);
    }

    private void GetAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        asteroid.transform.position = transform.position;
    }
    
    private Asteroid CreateAsteroid()
    {
        Asteroid asteroid = Instantiate(asteroidPrefab, GetRandomPosition(), quaternion.identity);
        asteroid.SetPool(_pool);
        return asteroid;
    }
    #endregion
    

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
        if (_pool.CountActive > maxAsteroidsOnScreen) return;
        
        Asteroid asteroid = _pool.Get();
        
        asteroid.transform.position = GetRandomPosition();
        asteroid.Initialize(GetRandomDirection(asteroid.transform.position), 
            GetRandomSize(), 
            GetRandomSpeed());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
    }

    Vector2 GetRandomPosition()
    {
        return Random.insideUnitCircle.normalized * spawnRadius;
    }
    
    Vector2 GetRandomDirection(Vector3 origin)
    {
        return (transform.position - origin).normalized;
    }

    float GetRandomSize()
    {
        return Random.Range(minSize, maxSize);
    }
    
    private float GetRandomSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }
}
