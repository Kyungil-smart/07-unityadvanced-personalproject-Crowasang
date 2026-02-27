using UnityEngine;

public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
    void Merge(FruitObject other);
}
