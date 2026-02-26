using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FruitSpawn : MonoBehaviour
{
    [SerializeField] private FruitData[] spawnableFruits;
    private FruitData _currentFruit;
    public PlayerInput _playerInput;
    public bool isReady = true;
    private Vector2 _mouseInputPos;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        GetNextFruit();
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
        _playerInput.Player.Drop.started += OnDrop;
    }

    private void OnDisable()
    {
        _playerInput.Player.Drop.started -= OnDrop;
        _playerInput.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _mouseInputPos = ctx.ReadValue<Vector2>();
    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if (isReady)
        {
            Debug.Log("클릭");
            SpawnFruit();
        }
    }

    private void SpawnFruit()
    {
        PoolManager.Instance.Get(_currentFruit, transform.position);
    }
    
    private void GetNextFruit()
    {
        _currentFruit = spawnableFruits[Random.Range(0, spawnableFruits.Length)];
    }
}
