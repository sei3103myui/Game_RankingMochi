using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MochiController : MochiBase
{
    public int enemyNumber = 0;
    public bool isAttackMode = false;
    public AudioClip seClip = default;
    public AudioClip upClip = default;

    private Text ScoreText;
    private bool isAttacktime = false;
    private bool ishit = false;//叩かれたかどうか管理するフラグ
    
    
  
    void Start()
    {
        
        GameObject obj = GameObject.FindGameObjectWithTag("Score");

        ScoreText = obj.GetComponent<Text>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isAttackMode)
        {
            if (!isAttacktime)
            {
                isAttacktime = true;
                StartCoroutine(TimeCoroutine());
                StartCoroutine(AttackCorutine());
            }
           
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ishit = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ishit = false;
        
    }
    public IEnumerator AttackCorutine()
    {
        int num = 0;
        while (isAttacktime)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && ishit)
            {
                num++;
                AudioManager2D.Instance.AudioSE.PlayOneShot(seClip);
                yield return null;
                if (num == Life)
                {
                    AudioManager2D.Instance.AudioSE.PlayOneShot(upClip);
                    
                    //スコア加算処理
                    gameManager.GameScore += Point;
                    ScoreText.text = gameManager.GameScore.ToString();

                    num = 0;//数値初期化
                    isAttackMode = false;
                    isAttacktime = false;
                    gameManager.AnimationEnd(enemyNumber);
                    break;
                }

            }
            yield return null;
            
        }     
             
    }

    private IEnumerator TimeCoroutine()
    {
        yield return new WaitForSeconds(HitTime);

        isAttackMode = false;
        isAttacktime = false;
    }
}
