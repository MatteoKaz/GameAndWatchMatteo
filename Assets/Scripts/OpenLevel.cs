using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadANewLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("sss");
    }
    public void Quitt()
    {
        Application.Quit();
    }
    public void LoadSameLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex );
        Debug.Log("sss");
    }
}
