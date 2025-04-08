using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class StartCanvas : MonoBehaviour
{
    public Canvas START_GAME_Canvas;//开始画布
    public Canvas LIAO_TIAN_Canvas;//聊天画布

    public Button StartGame;//分支选择按钮
    public Button CountinueGame;
    public Button GameSet;
    public Button ExitGame;//退出游戏按钮

    

    // Start is called before the first frame update
    void Start()
    {
        StartGame.onClick.AddListener(StartGame_OnClick);    
        ExitGame.onClick.AddListener(ExitGame_OnClick);


        RectTransform buttonRect = StartGame.GetComponent<RectTransform>();
        StartScaleAnimation(buttonRect); // 开始循环缩放动画

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartScaleAnimation(RectTransform buttonRect)
    {
        buttonRect.DOScale(1.8f, 1.4f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // 无限循环
    }

    private void StartGame_OnClick()
    {
        START_GAME_Canvas.gameObject.SetActive(false);
        LIAO_TIAN_Canvas.gameObject .SetActive(true);  
    }


    private void ExitGame_OnClick()
    {
        EditorApplication.isPlaying = false; // 退出 Play 模式

    }
}
