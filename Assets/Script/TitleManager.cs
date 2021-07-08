using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayLevel
{
    Easy,
    Normal,
    Hard
}

public class DifficultyLevel
{
    public int life;//必要ヒット回数
    public int hitTime;//ヒット可能時間
    public int getPoint = 0;//叩くのに成功したとき獲得できるポイント
    public int timelimit = 0;//制限時間
    public float attackInterval = 0;//Enemyが叩けるようになる間隔
    public string gameModeName = default;

    public DifficultyLevel(int life, int hitTime, int getpoint, int time ,float attacktime , string gamemode)
    {
        this.life = life;
        this.hitTime = hitTime;
        this.getPoint = getpoint;
        this.timelimit = time;
        this.attackInterval = attacktime;
        this.gameModeName = gamemode;
    }
}

public class GameConfig
{
    public static readonly DifficultyLevel Easy = new DifficultyLevel(1, 3, 10, 30,1.5f,"Easy");
    public static readonly DifficultyLevel Normal = new DifficultyLevel(2, 2, 50, 30,0.8f,"Normal");
    public static readonly DifficultyLevel Hard = new DifficultyLevel(3, 1, 100, 60,0.2f,"Hard");

    public static DifficultyLevel DifficultyLebel { get; set; } = Normal;
}

public class TitleManager : MonoBehaviour
{
    public GameObject modeSelectObj;
    public GameObject startPanelObj;
    public GameObject setRankPanelObj;

    public Text checkText;
    public AudioClip titleBgmClip;
    public GameObject[] firstBtn;
    // テンプレート
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstBtn[0]);
        AudioManager2D.Instance.AudioBgm.clip = titleBgmClip;
        AudioManager2D.Instance.AudioBgm.Play();
    }

    void Update()
    {

    }
    public void OnPlayLevel(int level)
    {
        switch ((PlayLevel)level)
        {
            case PlayLevel.Easy:
                GameConfig.DifficultyLebel = GameConfig.Easy;
                
                break;

            case PlayLevel.Normal:
                GameConfig.DifficultyLebel = GameConfig.Normal;
                
                break;

            case PlayLevel.Hard:
                GameConfig.DifficultyLebel = GameConfig.Hard;
                
                break;
        }
        setRankPanelObj.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstBtn[0]);
    }

    public void OnSetPlayLevel(int playLevel)
    {
        switch ((PlayLevel)playLevel)
        {
            case PlayLevel.Easy:
                GameConfig.DifficultyLebel = GameConfig.Easy;
                checkText.text = "難易度：Easyで始めますか？";
                break;

            case PlayLevel.Normal:
                GameConfig.DifficultyLebel = GameConfig.Normal;
                checkText.text = "難易度：Normalで始めますか？";
                break;

            case PlayLevel.Hard:
                GameConfig.DifficultyLebel = GameConfig.Hard;
                checkText.text = "難易度：Hardで始めますか？";
                break;
        }
    }

    public void OnGameStartBtn()
    {
        SceneManager.LoadScene("MainGame");
        AudioManager2D.Instance.AudioBgm.Stop();
    }


    public void OnSelectBtn()
    {
        modeSelectObj.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstBtn[1]);
    }

    public void OnSelectDifficultBtn()
    {
        startPanelObj.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstBtn[2]);
    }

    public void OnSelectRankBtn()
    {
        setRankPanelObj.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstBtn[3]);
    }
    public void OnBackBtn(int number)
    {
        switch (number)
        {
            case 0:
                modeSelectObj.SetActive(false);
                EventSystem.current.SetSelectedGameObject(firstBtn[0]);
                break;
            case 1:
                startPanelObj.SetActive(false);
                EventSystem.current.SetSelectedGameObject(firstBtn[1]);
                break;
            case 2:
                setRankPanelObj.SetActive(false);
                EventSystem.current.SetSelectedGameObject(firstBtn[0]);
                break;
        }
       
    }

    public void OnEndBtn()
    {
        //ゲーム終了
        Application.Quit();
    }
}
