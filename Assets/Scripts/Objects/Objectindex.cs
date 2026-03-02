using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
public class Objectindex : MonoBehaviour
{

    public ObjectMovement _ObjectMovement;
    private int _index = -1;
    [SerializeField]  private float _LifeDuration = 7f;
    private GameObject _gameObject;

    public void Init(ObjectMovement movement)
    {
        _ObjectMovement = movement;
        _ObjectMovement.OnMove += Movement;
        _ObjectMovement.OnMoveReverse += ReverseMovement;
    }

    private void OnDisable()
    {
        _ObjectMovement.OnMove -= Movement;
        _ObjectMovement.OnMoveReverse -= ReverseMovement;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(SpendingTime());  
    }

    IEnumerator SpendingTime()
    {

        yield return new WaitForSeconds(_LifeDuration);
        Debug.Log("je meurs");
        Destroy(gameObject);
       

    }


            // Update is called once per frame
    void Update()
    {
        
    }

    public void  Movement()
    {
        if (_ObjectMovement == null)
        {
            Debug.LogWarning("Object is null, cannot move");
            return;
        }
        Debug.LogWarning($"Object moved to position {_index + 1}");
        _index++;
        if (_index < _ObjectMovement._transforms.Length)
        {
            
            transform.position = _ObjectMovement._transforms[_index].position;
            //_audioEventDispatcher.PlayAudio(_ObjectMovementAudioType);
        }
        else
        {
            _index = -0;
            transform.position = _ObjectMovement._transforms[_index].position;
        }
    }

    private void ReverseMovement()
    {
        if (_ObjectMovement == null)
        {
            Debug.LogWarning("Object is null, cannot move");
            return;
        }
        Debug.LogWarning($"Object moved to position {_index + 1}");

        _index--;
        _index = Mathf.Clamp(_index,-1,999);
        if (_index > -1 )
        {

            transform.position = _ObjectMovement._transforms[_index].position;
            //_audioEventDispatcher.PlayAudio(_ObjectMovementAudioType);
        }
        else
        {
            _index = _ObjectMovement._transforms.Length -1 ;
            transform.position = _ObjectMovement._transforms[_index].position;
        }

    }
}
