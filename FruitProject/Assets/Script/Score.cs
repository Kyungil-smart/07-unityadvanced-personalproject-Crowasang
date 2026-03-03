using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;
    private TextMeshProUGUI scoreText;
    private int _score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }

    public void ScoreUpdate(int score)
    {
        _score += score;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score : {_score}";
    }
}
