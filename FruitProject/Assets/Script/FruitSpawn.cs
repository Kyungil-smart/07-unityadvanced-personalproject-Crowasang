using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FruitSpawn : MonoBehaviour
{
    [SerializeField] private FruitData[] spawnableFruits;
    private FruitData _currentFruit;
    public PlayerFruit _playerInput;
    public bool isReady = true;
    private Vector2 _mouseInputPos;

    private void Awake()
    {
        _playerInput = new PlayerFruit();
        GetNextFruit();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Drop.started += OnDrop;
    }

    private void OnDisable()
    {
        _playerInput.Player.Drop.started -= OnDrop;
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

    private void SpawnFruit()
    {
        PoolManager.Instance.Get(_currentFruit, transform.position);
        GetNextFruit();
    }
    
    private void GetNextFruit()
    {
        _currentFruit = spawnableFruits[Random.Range(0, spawnableFruits.Length)];
    }
}
