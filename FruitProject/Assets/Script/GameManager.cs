using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TextMeshProUGUI _lastScoreText;
    private FruitObject _nextFruit;
    [SerializeField] private FruitData[] allFruitData;
    [SerializeField] private GameObject _gameoverPanel;
    
    private void Awake() => Instance = this;

    private void Start()
    {
        _lastScoreText = _gameoverPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SpawnNextFruit(FruitData currentData, Vector3 spawnPos)
    {
        if (currentData._nextFruit != null)
        {
            PoolManager.Instance.Get(currentData._nextFruit, spawnPos);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _lastScoreText.text = $"My Score : {Score.Instance.LastScore()}";
        _gameoverPanel.SetActive(true);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
