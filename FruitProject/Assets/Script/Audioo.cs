using UnityEngine;

public class Audioo : MonoBehaviour
{
    private GameObject _originPrefab;
    private AudioSource _source;
    public void Setup(GameObject prefab, AudioClip clip, float vol, float pitch)
    {
        _originPrefab = prefab;
        _source.clip = clip;
        _source.volume = vol;
        _source.pitch = pitch;
        _source.Play();
    }
    
}
