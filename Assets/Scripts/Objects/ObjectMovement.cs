using System;

using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] public Transform[] _transforms;
    private int _index = -1;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private GameObject _ObjectFalling;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    
    [SerializeField] private AudioType _ObjectMovementAudioType;
    [SerializeField] private AudioType _ObjectDestructionAudioType;
    private bool _IsReverse = false;

    public event Action OnMove;
    public event Action OnMoveReverse;
    public void Init(GameObject NewObject)
    {
        _ObjectFalling = NewObject;
        _ObjectFalling.GetComponent<Objectindex>()._ObjectMovement = this;
        var index = _ObjectFalling.GetComponent<Objectindex>();
        index.Init(this);
        _ObjectFalling.GetComponent<Objectindex>().Movement();



    }
    private void OnEnable()
    {
        _timeManager.OnTimePassed += MoveObject;
        _timeManager.OnTimeReverse += ReverseObject;
    }

    private void OnDisable()
    {
        _timeManager.OnTimePassed -= MoveObject;
        _timeManager.OnTimeReverse -= ReverseObject;
    }

    private void MoveObject()
    {
        if (_IsReverse == false)
        {
            OnMove?.Invoke();
            
        }
        else
        {
            OnMoveReverse?.Invoke();
        }
            
      
    }
    private void ReverseObject()
    {
        if ( _IsReverse == false)
        {
            _IsReverse = true;  
        }
        else
        {
            _IsReverse = false;
        }
    }
}