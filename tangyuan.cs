using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tangyuan : MonoBehaviour
{
    private Animator animator;

    private bool isAnimationFinished = false;

    public Button startAnim;

    void Start()
    {
        animator = GetComponent<Animator>();
        startAnim.onClick.AddListener(PlayAnim);

        animator.speed = 0; // 暂停动画
    }


    private void PlayAnim()
    {

        animator.speed = 1; // 继续播放



    }

    public void OnAnimationEnd()
    {
        animator.SetTrigger("Exit"); // 触发退出动画
        animator.speed = 0; // 暂停动画
    }

    void Update()
    {        

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("MyAnimationClip") && stateInfo.normalizedTime >= 1.0f && !isAnimationFinished)
        {
            isAnimationFinished = true;
            Debug.Log("动画播放完成！");

        }
    }
}
