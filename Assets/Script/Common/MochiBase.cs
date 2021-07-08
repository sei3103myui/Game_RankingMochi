using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MochiBase : MonoBehaviour
{

    protected readonly int HitTime = GameConfig.DifficultyLebel.hitTime;
    protected readonly int Life = GameConfig.DifficultyLebel.life;
    protected readonly int Point = GameConfig.DifficultyLebel.getPoint;


    protected MochiGameManager gameManager;
    

    private void Awake()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = manager.GetComponent<MochiGameManager>();
    }

   
}
