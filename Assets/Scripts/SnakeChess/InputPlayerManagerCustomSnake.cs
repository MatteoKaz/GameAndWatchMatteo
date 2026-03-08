using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class InputPlayerManagerCustomSnake : MonoBehaviour
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
    [SerializeField] Camera mainCamera;

    public event Action<Cell> OnMove ;


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
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touch.screenPosition);
        Vector2 touchPos2D = new Vector2(worldPos.x, worldPos.y);
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
           

            RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                    CellClicked(cell.coord);
                OnMove?.Invoke(cell);

            }
            startPosition = touch.screenPosition;
            
          
        }
        if(touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            endPosition = touch.screenPosition;
            OnSwipe();
            

         

        }

      




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
    
    public void MoveLeft()
    {
        OnMoveLeft?.Invoke();
    }

    public void MoveRight()
    {
        OnMoveRight?.Invoke();
        
    }
    void CellClicked(Vector2Int coord)
    {
        // logique pour déplacer le serpent, vérifier validité, score, etc.
        Debug.Log("Cell clicked: " + coord);
    }
}
