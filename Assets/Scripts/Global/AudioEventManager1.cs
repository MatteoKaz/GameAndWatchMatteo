using UnityEngine;

public class AudioEventManager1 : MonoBehaviour
{
    [SerializeField] private AudioEventDispatcher1 audioEventDispatcher;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        audioEventDispatcher.OnAudioEvent += PlayAudioFX;
    }

    private void OnDisable()
    {
        audioEventDispatcher.OnAudioEvent -= PlayAudioFX;
    }


    private void PlayAudioFX(AudioClip clip)
    {
        
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
