using System;
using UnityEngine;

public class FruitObject : MonoBehaviour, IPoolable
{
    [HideInInspector] public FruitData data;
    public bool _isFruit;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] private float _gameoverTime = 0f;
    [SerializeField] private bool _isLand = true;
    [SerializeField] private GameObject _mergeParticle;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _isFruit = true;
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
        _isFruit = true;
        _isLand = true;
    }

    public void OnDespawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
        transform.rotation = Quaternion.identity;
        _gameoverTime = 0f;
        _isFruit = true;
        _isLand = true;
    }

    public void Merge(FruitObject other)
    {
        Vector3 mergePos = (transform.position + other.transform.position) / 2f;
        GameObject particle = Instantiate(_mergeParticle, mergePos, Quaternion.identity);
        PoolManager.Instance.Release(data, gameObject);
        PoolManager.Instance.Release(other.data, other.gameObject);
        GameManager.Instance.SpawnNextFruit(data, mergePos);
    }

    private void FixedUpdate()
    {
        if (!_isFruit && !_isLand)
        {
            _gameoverTime += Time.deltaTime;
            if (_gameoverTime >= 3f) GameManager.Instance.GameOver();
        }
        else
        {
            _gameoverTime = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit") || collision.gameObject.CompareTag("Floor"))
        {
            _isLand = false;
        }
        if (collision.gameObject.TryGetComponent(out FruitObject other))
        {
            if ((data._id == other.data._id) && (GetInstanceID() > other.GetInstanceID()))
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/Merge");
                AudioManager.Instance.PlaySound(clip, 0.1f);
                Score.Instance.ScoreUpdate(data._score);
                Merge(other); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isFruit = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isFruit = true;
    }
}
