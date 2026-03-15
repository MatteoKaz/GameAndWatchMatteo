using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class PlayerEat : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] public AIManger aim;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] public PlayerScoreSnake playerscore;
    [SerializeField] private SnakeBody snakeBody;
    private bool HasKill = false;
    public int movetoLooseMult = 1;
    public int BasemovetoLooseMult = 1;


    public event Action Eat;
    public event Action  Move;
    public event Action End;
    public List<SpriteRenderer> deathPlace;
    public List<TMP_Text> TMP_Texts;
    public int temporarPoint;
    public int indexPlaceToDie = 0;
    public EnemyMovement.MoveType moveType;
    public List<Sprite> ListOfSprite;
    public List<Animator> animatorsDeadpiece;
    
    void OnEnable()
    {

    }

    public void PlayerKill()
    {
        HasKill = false;
        for (int i = aim.enemies.Count - 1; i >= 0; i--)
        {
            if (aim.enemies[i].CurrentcoordEnemy == pm.coordPlayer)
            {
                pm.currentMoveType = (PlayerMovement.MoveType)aim.enemies[i].currentMoveType;

                // Launch effect et point ici

                EnemyMovement em = aim.enemies[i];
                aim.enemies.RemoveAt(i);
                temporarPoint = em.Value;
                moveType = em.currentMoveType;
                HasKill = true;
                playerscore.AddPoint(em.Value);
                pm.snakeBody.Grow();

               
                Eat?.Invoke();
                Destroy(em.gameObject);
            }
           
            
        }
        if (HasKill == true)
        {
            temporarPoint *= (int)playerscore.multiplicatorEnchainement;
            playerscore.multiplicatorEnchainement += playerscore.multiplierValueEnchainementValue;
            Debug.Log(playerscore.multiplicatorEnchainement);
            movetoLooseMult = BasemovetoLooseMult;

            SpawnEnemyDead();
            for (int i = 1; i < snakeBody.segments.Count; i ++)
            {
                Animator snakeAnim = snakeBody.segments[i].GetComponent<Animator>();
                snakeAnim.SetTrigger("Combo");
                snakeAnim.ResetTrigger("ComboEnd");
               
                //SonIci
            }

            
        }
        else
        {
            if (movetoLooseMult == 0)
            {
                playerscore.multiplicatorEnchainement = playerscore.multiplicatorEnchainementBaseValue;
                movetoLooseMult = BasemovetoLooseMult;
                //Son perte Combo
                for (int i = 1; i < snakeBody.segments.Count; i++)
                {
                    Animator snakeAnim = snakeBody.segments[i].GetComponent<Animator>();
                    snakeAnim.SetTrigger("BackToIdle");
                   
                    //SonIci
                }

            }
            if (movetoLooseMult > 0)
            {
                
                if (movetoLooseMult == 1)
                {
                    for (int i = 1; i < snakeBody.segments.Count; i++)
                    {
                        Animator snakeAnim = snakeBody.segments[i].GetComponent<Animator>();
                        snakeAnim.SetTrigger("ComboEnd");
                        
                        //SonIci
                    }
                }
                movetoLooseMult -= 1;
            }
            
          

        }
        Move?.Invoke();
        End?.Invoke();
    }

    public void CutOrKillPlayer(Vector2Int pos)
    {
        for (int i = 0; i < snakeBody.snakeCoords.Count; i++)
            if(pos == snakeBody.snakeCoords[i])
            {
                snakeBody.RemoveSegmentAt(pos);
            }

       
    }

    public void SpawnEnemyDead()
    {
        TMP_Texts[indexPlaceToDie].text = $"+{temporarPoint}";
        temporarPoint = 0;
        switch (moveType)
        {
            case EnemyMovement.MoveType.Cavalier:
                 deathPlace[indexPlaceToDie].sprite = ListOfSprite[0];
                break;
            case EnemyMovement.MoveType.Roi:
                deathPlace[indexPlaceToDie].sprite = ListOfSprite[1];
                break;
            case EnemyMovement.MoveType.Fou:
                deathPlace[indexPlaceToDie].sprite = ListOfSprite[2];
                break;
            case EnemyMovement.MoveType.Tour:
                deathPlace[indexPlaceToDie].sprite = ListOfSprite[3];
                break;
            case EnemyMovement.MoveType.Dame:
                deathPlace[indexPlaceToDie].sprite = ListOfSprite[4];
                break;

        }
        indexPlaceToDie++;


    }

}
