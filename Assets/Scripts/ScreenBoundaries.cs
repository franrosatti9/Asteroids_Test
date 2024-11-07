using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    private Vector2 boundaries;

    private void Awake()
    {
        Setup();
    }

    [ContextMenu("Setup")]
    void Setup()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        
        float height = Camera.main.orthographicSize * 2;
        boundaries = new Vector2(height * Camera.main.aspect, height);
        _boxCollider.size = boundaries;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out WraparoundObject wraparoundObject))
        {
            Vector2 newPos = CalculateNewPosition(wraparoundObject.transform.position, other.bounds.size);    
            wraparoundObject.transform.position = newPos;
        }
    }

    Vector2 CalculateNewPosition(Vector2 pos, Vector2 objectSize)
    {
        float dir = 0;
        if (CheckOutOfBoundsX(pos.x, out dir))
        {
            Debug.Log("Out of bounds X");
            return new Vector2(pos.x * -1, pos.y) + new Vector2(objectSize.x * .5f * dir, 0);
        }
        else if (CheckOutOfBoundsY(pos.y, out dir))
        {
            Debug.Log("Out of bounds Y");
            return new Vector2(pos.x, pos.y * -1) + new Vector2(0, objectSize.y * .5f * dir);
        }
        else
        {
            Debug.Log("Not out of bounds");
            Debug.Log($"X diff: {Mathf.Abs(pos.x) - Mathf.Abs(boundaries.x * .5f)}, Y Diff: {Mathf.Abs(pos.y) - Mathf.Abs(boundaries.y) * .5f}");
            return pos;
        }
        
    }

    private bool CheckOutOfBoundsY(float yPos, out float dir)
    {
        float diff = Mathf.Abs(yPos) - Mathf.Abs(boundaries.y * .5f);
        dir = Mathf.Sign(yPos);
        if (diff > 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckOutOfBoundsX(float xPos, out float dir)
    {
        
        float diff = Mathf.Abs(xPos) - Mathf.Abs(boundaries.x * .5f);
        dir = Mathf.Sign(xPos);
        if (diff > 0)
        {
            return true;
        }
        return false;
    }
}
