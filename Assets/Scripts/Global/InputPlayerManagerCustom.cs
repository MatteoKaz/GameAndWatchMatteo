using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class InputPlayerManagerCustom : MonoBehaviour
{
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    [SerializeField] private float _tapDuration = 1.0f;
    private float _tapTimer = 0.0f;
    private bool _isTouching = false;
    private float width = 0.0f;
    private float height = 0.0f;

    private Vector2 startPosition;
    string debugText = "";
    private Vector2 endPosition;


    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;

        


    }

    //public void OnTap()
    //{
    //    Debug.Log("TAP");
    //}

    private void OnSwipe()
    {
        Vector2 delta = endPosition - startPosition;
        delta = delta.normalized;
        float dot = Vector2.Dot(delta, Vector2.up); 

        if (Mathf.Abs(dot) > 0.7f)
        {
            if (dot < 0)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
                
            }
        }
    }




    private void Update()
    {
        if(Touch.activeTouches.Count == 0)
        {
            return;
        }
        var touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startPosition = touch.screenPosition;
            debugText = "Touches: " + Touch.activeTouches.Count;
            Debug.Log("Start");
          
        }
        if(touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            endPosition = touch.screenPosition;
            OnSwipe();
            debugText = "Touchesend: " + Touch.activeTouches.Count;
            Debug.Log("end");

         

        }

        Debug.Log(Touch.activeTouches.Count);




        //if (Input.touchCount > 0)
        //{
        //    Touch firstTouch = Input.GetTouch(0);

        //    if (firstTouch.phase == TouchPhase.Began)
        //    {
        //        _isTouching = true;
        //    }
        //    else if (firstTouch.phase == TouchPhase.Ended)
        //    {
        //        _isTouching = false;
        //        if (_tapTimer <= _tapDuration)
        //        {
        //            Debug.LogWarning($"Tap detected, Touch at {firstTouch.position}");

        //            if (firstTouch.position.x < width / 2)
        //            {
        //                Debug.LogWarning("Tap Right");
        //            }
        //            else
        //            {
        //                Debug.LogWarning("Tap Left");
        //            }
        //            _tapTimer = 0.0f;

        //        }

        //    }
        //    if (_isTouching)
        //    {
        //        _tapTimer += Time.deltaTime;
        //    }


        //    if (Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        MoveRight();
        //    }

        //    if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    {
        //        MoveLeft();

        //    }
        //}
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1500, 1500), debugText);
    }
    public void MoveLeft()
    {
        OnMoveLeft?.Invoke();
    }

    public void MoveRight()
    {
        OnMoveRight?.Invoke();
        
    }
}
