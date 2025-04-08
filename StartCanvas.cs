using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class StartCanvas : MonoBehaviour
{
    public Canvas START_GAME_Canvas;//��ʼ����
    public Canvas LIAO_TIAN_Canvas;//���컭��

    public Button StartGame;//��֧ѡ��ť
    public Button CountinueGame;
    public Button GameSet;
    public Button ExitGame;//�˳���Ϸ��ť

    

    // Start is called before the first frame update
    void Start()
    {
        StartGame.onClick.AddListener(StartGame_OnClick);    
        ExitGame.onClick.AddListener(ExitGame_OnClick);


        RectTransform buttonRect = StartGame.GetComponent<RectTransform>();
        StartScaleAnimation(buttonRect); // ��ʼѭ�����Ŷ���

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartScaleAnimation(RectTransform buttonRect)
    {
        buttonRect.DOScale(1.8f, 1.4f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // ����ѭ��
    }

    private void StartGame_OnClick()
    {
        START_GAME_Canvas.gameObject.SetActive(false);
        LIAO_TIAN_Canvas.gameObject .SetActive(true);  
    }


    private void ExitGame_OnClick()
    {
        EditorApplication.isPlaying = false; // �˳� Play ģʽ

    }
}
