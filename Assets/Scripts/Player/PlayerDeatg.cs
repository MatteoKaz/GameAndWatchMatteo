using UnityEngine;

public class PlayerDeatg : MonoBehaviour
{
    [SerializeField] TimeManager timeManager;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    [SerializeField] PlayerScore PlayerScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<Bonus>() == null)
        {
            timeManager.StopTime();
            _audioEventDispatcher.PlayAudio(AudioType.Death);
            DeathMenu.SetActive(true);
            PlayerScore.StopAllCoroutines();


        }
        
    }
}
