using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // SAFE CHECK IN CASE SOMETHING LEAVES THE SCREEN 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out WraparoundObject wraparoundObject)) return;
        
        if(other.gameObject.TryGetComponent(out IPoolObject poolObject))
        {
            if(other.gameObject.activeInHierarchy) poolObject.ReleaseToPool();
        }
        else if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            player.transform.position = Vector2.zero;
        }
    }
}
