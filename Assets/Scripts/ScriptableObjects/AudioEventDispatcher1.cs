using System;
using UnityEngine;

public enum AudioType1
{
    None,
    Eat,
    PlayerMovement,
    Click,
    LoostPart,
    Win,
    Point,
}

[Serializable]
public struct AudioInfos1
{ 
    public AudioType audioType;
    public AudioClip audioClip;  
}

[CreateAssetMenu(fileName = "AudioEventDispatcher1", menuName = "Scriptable Objects/AudioEventDispatcher1")]
public class AudioEventDispatcher1 : ScriptableObject
{
    [SerializeField] private AudioInfos[] _audioClips;


    public event Action<AudioClip> OnAudioEvent;


    public void PlayAudio(AudioType audioType)
    {
        for (int i = 0; i < _audioClips.Length; i++)
        {
            if (_audioClips[i].audioType == audioType)
            {
               OnAudioEvent?.Invoke(_audioClips[i].audioClip);
                return;
            }
        }
      
    }
}
