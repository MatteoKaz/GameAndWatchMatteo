using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private Transform[] m_transforms;
    [SerializeField] private InputPlayerManagerCustom m_inputPlayerManager;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;

    private int m_currentIndex = 4;
    private int m_moveSpeed = 1;


    private void OnEnable()
    {
        m_inputPlayerManager.OnMoveLeft += MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight += MoveToNextPosition;
    }
    
    private void OnDisable()
    {
        m_inputPlayerManager.OnMoveLeft -= MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight -= MoveToNextPosition;
    }

    private void Start()
    {
        m_currentIndex = 4;
        transform.position = m_transforms[m_currentIndex].position;
    }

    public void MoveToNextPosition()
    {
       m_currentIndex += m_moveSpeed;
       m_currentIndex = Mathf.Clamp(m_currentIndex, 0, m_transforms.Length-1);
       UpdatePosition();
       
    }

    public void MoveToPrevPosition()
    {
        m_currentIndex -= m_moveSpeed;
        m_currentIndex = Mathf.Clamp(m_currentIndex, 0, m_transforms.Length-1);
        UpdatePosition();
    }

    public void MoveToDirection(int direction) //direction = -1 OU 1
    {
        m_currentIndex = m_currentIndex + m_moveSpeed*direction;
        m_currentIndex = Mathf.Clamp(m_currentIndex, 0, m_transforms.Length-1);
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.position = m_transforms[m_currentIndex].position;
        _audioEventDispatcher.PlayAudio(AudioType.PlayerMovement);
    }
}
