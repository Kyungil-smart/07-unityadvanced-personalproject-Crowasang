using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NextFruit : MonoBehaviour
{
    public static NextFruit Instance;
    [SerializeField] private FruitData[] spawnFruits;
    [SerializeField] private SpriteRenderer previewRenderer;
    private FruitData _nextFruit;

    private void Awake()
    {
        Instance = this;
        SetNextFruit();
    }

    private void SetNextFruit()
    {
        _nextFruit = spawnFruits[Random.Range(0, spawnFruits.Length)];
        previewRenderer.sprite = _nextFruit._sprite;
        previewRenderer.transform.localScale = Vector3.one * _nextFruit._fruitScale;
    }
    
    public FruitData GetNextFruit()
    {
        FruitData returnFruit = _nextFruit;
        SetNextFruit();
        return returnFruit;
    }
    
}
