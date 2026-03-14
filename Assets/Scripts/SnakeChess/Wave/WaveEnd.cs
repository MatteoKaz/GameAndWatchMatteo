using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class WaveEnd : MonoBehaviour
{
    [SerializeField] public PlayerEat pe;
    [SerializeField] public PlayerMovement pm;
    [SerializeField] public AIManger aim;
    [SerializeField] public PlayerScoreSnake playerScore;
    [SerializeField] public GridManager gm;
    [SerializeField] public WaveManager wm;
    [SerializeField] public UpgradeGiver updateGiver;
    [SerializeField] public SetUpgrade setUpgrade;
    public Animator animator;
    public bool waveActive = false;
    public event Action CloseMenuCard;
    [SerializeField] GameObject DeathScreen;
    public Animator animatorDeathScreen;
    public List<GameObject> DeathgameObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        pe.End += CheckEnd;
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void BeginWave()
    {
        waveActive = true;
        // spawn ennemis etc
    }

    public void CheckEnd()
    {

        if (!waveActive) return;
        Debug.Log($"CheckEnd called - enemies: {aim.enemies.Count}, NumberOfMoves: {pm.NumberOfMoves}");
        if (aim.enemies.Count == 0 || pm.NumberOfMoves <= 0)
        {
            playerScore.CalculateScore();
            if (wm.currentWaveData.waveValue <= playerScore.score)
            {
                
                Debug.Log("fin wave" );
                //Quand upgrade modifier ici 
                ResetAndRelaunch();
            }
            else
            {
                StartCoroutine(Death());
            }
        }

    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(12f);
        DeathScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        animatorDeathScreen.SetTrigger("WaveEnd");
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < DeathgameObjects.Count; i++)
        {
            DeathgameObjects[i].SetActive(true);
        }
    }





    public void ResetAndRelaunch()
    {


        StartCoroutine(EndWave());


    }
    
    public void LaunchNewWave()
    {
        if(waveActive == false)
        {
            StartCoroutine(BeginNewWave());
        }
        

    }
    public IEnumerator EndWave()
    {
        waveActive = false;
        
        yield return new WaitForSeconds(12f);
        playerScore.ResetValue();
        aim.ClearEnemies();
        yield return new WaitForSeconds(0.75f);
        pm.snakeBody.DestroySnake();
        yield return new WaitForSeconds(0.75f);
        animator.SetTrigger("WaveEnd");
        yield return new WaitForSeconds(1f);
        setUpgrade.GenerateUpgrade();



    }
    public IEnumerator BeginNewWave()
    {
        if(waveActive == false)
        {
            Debug.Log("Debut Enumerator");
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("ChoiceDone");
            yield return new WaitForSeconds(1.25f);
            gm.InvokeThings();
            while (gm.FinishInvoke == true)
             {
              yield return null;
             }
                yield return new WaitForSeconds(1f);
                wm.NextWave();
                yield return new WaitForSeconds(0.5f);
                aim.PlaceEnemyAim(); 
            
        }
                
        

    }

    public void CloseCard()
    {
        CloseMenuCard?.Invoke();
    }
}
