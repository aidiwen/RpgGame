using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ZoumadengGame : MonoBehaviour
{
    private RPGGameManager gameManager; // ������Ա����
    private Player playercontroller;
    private DialogueTrigger dialogueTrigger;

    public RawImage rawImage; // UI�ϵ�RawImage
    public VideoPlayer videoPlayer; // VideoPlayer���
    public Canvas ZOU_MA_DENG_Canvas;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//��ȡgameManagerʵ��        
        playercontroller = FindObjectOfType<Player>();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();

        // ��������
        videoPlayer.Play();

        // ������Ƶ��������¼�
        videoPlayer.loopPointReached += OnVideoFinished;

       
       
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("��Ƶ������ɣ�");
        // ������ִ������Ҫ���߼����������� RawImage ���л�����
        
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        playercontroller.Canmove = true;
    }

    public void SpeedUp()//���ٲ���
    {
        videoPlayer.playbackSpeed = 6.0f; // 2 ����
    }
}
