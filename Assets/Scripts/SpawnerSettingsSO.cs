using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnerSettingsSO", menuName = "SO/Spawner Settings")]
public class SpawnerSettingsSO : ScriptableObject
{
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private int maxAsteroidsOnScreen = 10;
    
    public float SpawnRate => spawnRate;
    public float MinSpeed => minSpeed;
    public float MaxSpeed => maxSpeed;
    public int MaxAsteroidsOnScreen => maxAsteroidsOnScreen;
}
