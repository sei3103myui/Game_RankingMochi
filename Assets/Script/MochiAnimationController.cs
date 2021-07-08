using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MochiAnimationController : MonoBehaviour
{
    public bool isPlay = false;

    private Animator myAnimator;
    private int time = 0;
    private bool isStart = false;
    void Start()
    {
        //自身のアニメーター取得
        myAnimator = gameObject.GetComponent<Animator>();

        time = GameConfig.DifficultyLebel.hitTime;
    }

    void Update()
    {
        if (!isStart && isPlay)
        {
            isStart = true;
            StartCoroutine(AnimationCroutine());
        }

        if (!isPlay && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("mochiAnimation"))
        {
            myAnimator.SetBool("On", false);
            isStart = false;
        }
    }

    public IEnumerator AnimationCroutine()
    {
        myAnimator.SetBool("On", true);
        yield return new WaitForSeconds(time);
        if (isPlay)
        {
            myAnimator.SetBool("On", false);
            isPlay = false;
            isStart = false;
        }
        
    }
}
