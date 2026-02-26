using System;
using UnityEngine;

public class FruitObject : MonoBehaviour, IPoolable
{
    public FruitData data;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rb;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(FruitData newData)
    {
        data = newData;
        _spriteRenderer.sprite = newData._sprite;
    }

    public void OnSpawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.simulated = true;
    }

    public void OnDespawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void Merge()
    {
        
    }
}
