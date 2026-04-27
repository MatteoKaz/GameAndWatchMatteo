using System;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType1
{
    None,
    Eat,
    PlayerMovement,
    Click,
    EnnemyMovement,
    LoostPart,
    Win,
    Death,
    Point,
    Score,
    ScoreDown,
    Grid,
    
}

[Serializable]
public struct AudioInfos1
{ 
    public AudioType1 audioType;
    public AudioClip audioClip;
    public float volume;
}

[CreateAssetMenu(fileName = "AudioEventDispatcher1", menuName = "Scriptable Objects/AudioEventDispatcher1")]
public class AudioEventDispatcher1 : ScriptableObject
{
    [SerializeField] private AudioInfos1[] _audioClips;


    public event Action<AudioClip,float> OnAudioEvent;

   
    public void PlayAudio(AudioType1 audioType)
    {
        for (int i = 0; i < _audioClips.Length; i++)
        {
            if (_audioClips[i].audioType == audioType)
            {
                
                OnAudioEvent?.Invoke(_audioClips[i].audioClip, _audioClips[i].volume);
                return;
            }
        }
      
    }
  
}
