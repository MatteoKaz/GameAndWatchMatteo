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
    [SerializeField] private PlayerEat playerEat;
  




    private void OnEnable()
    {
        scoreSnake.ShakeCam += LittleShake;
        scoreSnake.LittleShakeCam += ShakeEnemy;
        playerEat.Eat += ShakeEat;
        playerEat.Move += LittleShake;
        scoreSnake.MicroShakeCam += MicroShake;
        playerEat.enemyEat += ShakeEnemy;

    }

    
    private void OnDisable()
    {
       
    }

    void PreciseShake()
    {
        float y;

        if (Random.value < 0.5f)
        {
            y = Random.Range(-0.001f, -0.001f);
        }
        else
        {
            y = Random.Range(0.001f, 0.01f);
        }

        ShakeDirection = new Vector3(Random.Range(-0.07f, 0.07f), y, 0f);
        shakeDuration = 0.05f;
    }
    void ShakeEat()
    {
        ShakeDirection = new Vector3(Random.Range(-0.09f, 0.09f), Random.Range(-0.003f, 0.003f), 0f);
        shakeDuration = 0.05f;
    }
    void ShakeEnemy()
    {
        ShakeDirection = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.002f, 0.002f), 0f);
        shakeDuration = 0.05f;
    }
    void TimeShake()
    {
        ShakeDirection = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.002f, 0.002f), 0f);
        shakeDuration = 0.05f;
    }
    void LittleShake()
    {
        ShakeDirection = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.001f, 0.001f), 0f);
        shakeDuration = 0.04f;
    }
    void MicroShake()
    {
        ShakeDirection = new Vector3(Random.Range(-0.0023f, 0.0025f), Random.Range(-0.0013f, 0.0015f), 0f);
        shakeDuration = 0.03f;
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
