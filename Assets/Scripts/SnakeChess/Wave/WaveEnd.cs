using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WaveEnd : MonoBehaviour
{
    [SerializeField] public PlayerEat pe;
    [SerializeField] public PlayerMovement pm;
    [SerializeField] public AIManger aim;
    [SerializeField] public PlayerScoreSnake playerScore;
    [SerializeField] public GridManager gm;
    [SerializeField] public WaveManager wm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        pe.End += CheckEnd;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckEnd()
    {
        if (aim.enemies.Count == 0 || pm.NumberOfMoves == 0)
        {
            playerScore.CalculateScore();

            //Quand upgrade modifier ici 
            ResetAndRelaunch();
        }
    }

    public void ResetAndRelaunch()
    {
        
        
        StartCoroutine(GenerateGrid());
       

    }
    public IEnumerator GenerateGrid()
    {
        yield return new WaitForSeconds(10f);
        playerScore.ResetValue();
        aim.ClearEnemies();
        yield return new WaitForSeconds(0.75f);
        pm.snakeBody.DestroySnake();
      
        yield return new WaitForSeconds(0.75f);
        gm.InvokeThings();
        yield return new WaitForSeconds(0.75f);
            wm.NextWave();
        yield return new WaitForSeconds(0.25f);
        aim.PlaceEnemyAim();
    }
}
