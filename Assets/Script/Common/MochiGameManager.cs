using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MochiGameManager : MonoBehaviour
{
    public enum GameMode
    {
        Start,
        Play,
        Pause,
        End,
        Ranking
    }

    public static readonly int Timelimit = GameConfig.DifficultyLebel.timelimit;

    public GameMode gameMode = GameMode.Start;

    public int GameScore = 0;
    public List<GameObject> hits;
    public List<MochiAnimationController> enemysAnimator;
    public GameObject startPanel;
    public GameObject resultPanel;
    public GameObject finishPanel;
    public Text timeText;
    public Text endScoreText;
    public AudioClip bgmClip;

    private MochiController mochiController;
    private GameObject hitPos;
    private float countTime = 0;
    private float attackInterval = 0;
    private bool isControll = false;
    

    private void Awake()
    {
        SetEnemyNumber();
        SetAnimators();
        countTime = Timelimit;
        attackInterval = GameConfig.DifficultyLebel.attackInterval;

    }
    void Start()
    {
        
    }

    void Update()
    {
        
        if(gameMode == GameMode.Start && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            startPanel.SetActive(false);
            gameMode = GameMode.Play;
        }
        if(gameMode == GameMode.Play && !isControll)
        {
            isControll = true;
            AudioManager2D.Instance.AudioBgm.clip = bgmClip;
            AudioManager2D.Instance.AudioBgm.Play();
            StartCoroutine(MochiAttackCoroutine());
        }
        if(gameMode == GameMode.End)
        {
            if (!resultPanel.activeInHierarchy)
            {
                resultPanel.SetActive(true);
                AudioManager2D.Instance.AudioBgm.Stop();
                endScoreText.text = GameScore.ToString();
            }
        }
        if(gameMode == GameMode.Ranking)
        {
            if (!finishPanel.activeInHierarchy)
            {
                finishPanel.SetActive(true);
                resultPanel.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if(gameMode == GameMode.Play && countTime >= 0.0f)
        {
            countTime -= Time.fixedDeltaTime;
            int num = (int)countTime;
            timeText.text = num.ToString();
        }else if(countTime <= 0.0f && gameMode == GameMode.Play)
        {
            gameMode = GameMode.End;
        }
        
    }

    /// <summary>
    /// Enemy(mochi)に個々に番号を振る
    /// </summary>
    public void SetEnemyNumber()
    {
        hitPos = GameObject.FindGameObjectWithTag("Enemys");
        hits = new List<GameObject>();

        for(int i = 0; i < hitPos.transform.childCount; i++)
        {
            hits.Add(hitPos.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < hits.Count; i++)
        {
            hits[i].GetComponent<MochiController>().enemyNumber = i + 1;
        }
    }

    public void SetAnimators()
    {
        GameObject animEnemys = GameObject.FindGameObjectWithTag("Animators");
        enemysAnimator = new List<MochiAnimationController>();
        for(int i = 0; i < animEnemys.transform.childCount; i++)
        {
            enemysAnimator.Add(animEnemys.transform.GetChild(i).gameObject.GetComponent<MochiAnimationController>());
        }
    }

    public IEnumerator MochiAttackCoroutine()
    {
        while(gameMode == GameMode.Play)
        {
            int enemynum = Random.Range(0, hits.Count);
            
            if (!hits[enemynum].GetComponent<MochiController>().isAttackMode && !enemysAnimator[enemynum].isPlay)
            {
                //アタック判定オン
                mochiController = hits[enemynum].GetComponent<MochiController>();
                mochiController.isAttackMode = true;
                //アニメーションスタート
                enemysAnimator[enemynum].isPlay = true;
                
            }
            
            yield return new WaitForSeconds(attackInterval);
        }
        isControll = false;
    }

    public void AnimationEnd(int EnemyNum)
    {
        enemysAnimator[EnemyNum - 1].isPlay = false;   
    }

    public void OnTitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
