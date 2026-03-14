using UnityEngine;




public class CameraShakeSnake : MonoBehaviour
{

   
    [SerializeField] private Camera _CamRef;
    private Vector3 customRot;
    private Vector3 camPos;
    private float smoothtime = 0.1f;
    public float shakeDuration = 0f;

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    private Vector3 _offset = new Vector3(0f, 0f, 9f);
    public Vector3 velocity;
    private Vector3 ShakeDirection;


    [SerializeField] private PlayerScoreSnake scoreSnake;
  




    private void OnEnable()
    {
        scoreSnake.ShakeCam += TimeShake;
    }

    
    private void OnDisable()
    {
       
    }

    void TimeShake()
    {
        ShakeDirection = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.001f, 0.002f), 0f);
        shakeDuration = 0.1f;
    }
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _CamRef = GetComponent<Camera>();
        customRot = new(0f, 0f, 0f);
       camPos = _CamRef.transform.position;
}

    // Update is called once per frame
    void Update()
    {
        
        Vector3 targetPosition = new Vector3(camPos.x, camPos.y, camPos.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothtime);
        transform.rotation = Quaternion.Euler(customRot);
        if (shakeDuration > 0)
        {
            
            transform.position = transform.position + ShakeDirection * shakeAmount;
            //Random.insideUnitSphere
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            
            
            shakeDuration = 0f;

        }
    }
}
