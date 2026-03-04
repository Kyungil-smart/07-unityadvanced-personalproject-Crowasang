using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TextMeshProUGUI _lastScoreText;
    [SerializeField] private FruitData[] allFruitData;
    [SerializeField] private GameObject _gameoverPanel;
    
    private void Awake() => Instance = this;

    private void Start()
    {
        _lastScoreText = _gameoverPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SpawnNextLevel(FruitData currentData, Vector3 spawnPos)
    {
        if (currentData._nextFruit != null)
        {
            PoolManager.Instance.Get(currentData._nextFruit, spawnPos);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _lastScoreText.text = $"내 점수 : {Score.Instance.LastScore().ToString()}";
        _gameoverPanel.SetActive(true);
    }
}
