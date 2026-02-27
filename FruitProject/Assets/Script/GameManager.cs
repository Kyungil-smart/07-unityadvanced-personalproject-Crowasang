using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private FruitData[] allFruitData;
    
    private void Awake() => Instance = this;
    
    public void SpawnNextLevel(FruitData currentData, Vector3 spawnPos)
    {
        if (currentData._nextFruit != null)
        {
            PoolManager.Instance.Get(currentData._nextFruit, spawnPos);
        }
    }
}
