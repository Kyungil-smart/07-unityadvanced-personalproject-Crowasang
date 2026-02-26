using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Scriptable Objects/FruitData")]
public class FruitData : ScriptableObject
{
    [Header("기본 정보")] 
    public int _id;
    public string _name;

    [Header("이미지")] 
    public Sprite _sprite;
    public GameObject _fruitPrefab;

    [Header("규칙")] 
    public int _score;
    public FruitData _nextFruit;
    
}
