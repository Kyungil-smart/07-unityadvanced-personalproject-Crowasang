using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
    }
    
    public void PlaySound(AudioClip clip, float vol = 1.0f, float pitch = 1.0f)
    {
        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
