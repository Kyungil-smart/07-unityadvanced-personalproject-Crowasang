using System;
using UnityEngine;

public class FruitObject : MonoBehaviour, IPoolable
{
    [HideInInspector] public FruitData data;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rb;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
        transform.rotation = Quaternion.identity;
    }

    public void OnDespawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
        transform.rotation = Quaternion.identity;
    }

    public void Merge(FruitObject other)
    {
        Vector3 mergePos = (transform.position + other.transform.position) / 2f;
        PoolManager.Instance.Release(data, gameObject);
        PoolManager.Instance.Release(other.data, other.gameObject);
        GameManager.Instance.SpawnNextLevel(data, mergePos);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FruitObject other))
        {
            if ((data._id == other.data._id) && (GetInstanceID() > other.GetInstanceID()))
            {
                Merge(other); 
            }
        }
    }
}
