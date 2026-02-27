using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FruitSpawn : MonoBehaviour
{
    [SerializeField] private FruitData[] spawnFruits;
    [SerializeField] private SpriteRenderer previewRenderer;
    private FruitObject _fruitObject;
    private FruitData _currentFruit;
    public PlayerFruit _playerInput;
    public bool isReady = true;
    private Vector2 _mouseInputPos;

    private void Awake()
    {
        _playerInput = new PlayerFruit();
        _mouseInputPos = new Vector2();
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
            StartCoroutine(SpawnFruit());
        }
    }

    private void FollowFruit()
    {
        Vector3 worldPosX = Camera.main.ScreenToWorldPoint(_mouseInputPos);
        
        float targetX = Mathf.Clamp(worldPosX.x, -3.7f, 3.7f);
    
        transform.position = new Vector3(targetX, transform.position.y, 0);
    }
    
    private IEnumerator SpawnFruit()
    {
        isReady = false;
        PoolManager.Instance.Get(_currentFruit, transform.position);
        previewRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        previewRenderer.enabled = true;
        isReady = true;
        GetNextFruit();
    }
    
    private void GetNextFruit()
    {
        _currentFruit = spawnFruits[Random.Range(0, spawnFruits.Length)];
        previewRenderer.sprite = _currentFruit._sprite;
    }
}
