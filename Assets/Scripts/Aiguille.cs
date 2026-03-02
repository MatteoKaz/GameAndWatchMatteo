using UnityEngine;

public class Aiguille : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    private Vector3 customRot;
    private bool Reverse = false;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;

    private void OnEnable()
    {
        _timeManager.OnTimePassed += MoveClock;
        _timeManager.OnTimeReverse += MoveClockBackward;
    }

    private void OnDisable()
    {
        _timeManager.OnTimePassed -= MoveClock;
        _timeManager.OnTimeReverse -= MoveClockBackward;
    }


    void MoveClock()
    {
        
        if (Reverse == true)
        {
            customRot = new(0f, 0f, 15f);
            transform.rotation *= Quaternion.Euler(customRot);
            _audioEventDispatcher.PlayAudio(AudioType.ObjectMovement);
        }
        else
        {
            customRot = new(0f, 0f, 15f);
            transform.rotation *= Quaternion.Euler(-customRot);
            _audioEventDispatcher.PlayAudio(AudioType.ObjectMovement);
        }
    }
    void MoveClockBackward()
    {
        if (Reverse == false)
        {
            Reverse = true;
        }
        else
        {
            Reverse = false;
        }
    }

}
