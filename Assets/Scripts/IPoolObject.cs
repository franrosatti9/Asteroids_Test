using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolObject<T> where T : MonoBehaviour
{ 
    ObjectPool<T> Pool { get; }
    void Reset();
    void SetPool(ObjectPool<T> pool);

    void ReleaseToPool();
}
