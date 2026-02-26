using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FruitSpawn : MonoBehaviour
{
    [SerializeField] private FruitData[] spawnFruits;
    [SerializeField] private SpriteRenderer previewRenderer;
    private FruitData _currentFruit;
    public PlayerFruit _playerInput;
    public bool isReady = true;
    private Vector2 _mouseInputPos;

    private void Awake()
    {
        _playerInput = new PlayerFruit();
        GetNextFruit();
    }

    private void Update()
    {
        FollowFruit();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Drop.started += OnDrop;
    }

    private void OnDisable()
    {
        _playerInput.Player.Drop.started -= OnDrop;
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Disable();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _mouseInputPos = ctx.ReadValue<Vector2>();
    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isReady)
        {
            SpawnFruit();
        }
    }

    private void FollowFruit()
    {
        Vector3 mousePosWithZ = new Vector3(_mouseInputPos.x, _mouseInputPos.y, 10f);
        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosWithZ);
        
        float targetX = Mathf.Clamp(worldPos.x, -2.8f, 2.8f);
    
        transform.position = new Vector3(targetX, 3.5f, 0);
    }
    
    private void SpawnFruit()
    {
        PoolManager.Instance.Get(_currentFruit, transform.position);
        GetNextFruit();
    }
    
    private void GetNextFruit()
    {
        _currentFruit = spawnFruits[Random.Range(0, spawnFruits.Length)];
        previewRenderer.sprite = _currentFruit._sprite;
    }
}
