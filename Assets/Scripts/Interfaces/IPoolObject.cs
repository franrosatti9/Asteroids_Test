using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolObject<T> : IPoolObject where T : MonoBehaviour
{ 
    ObjectPool<T> Pool { get; }
    
    void SetPool(ObjectPool<T> pool);
}

public interface IPoolObject
{
    void Reset();
    
    void ReleaseToPool();
}
