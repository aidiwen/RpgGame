using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ZoumadengGame : MonoBehaviour
{
    private RPGGameManager gameManager; // 声明成员变量
    private Player playercontroller;
    private DialogueTrigger dialogueTrigger;

    public RawImage rawImage; // UI上的RawImage
    public VideoPlayer videoPlayer; // VideoPlayer组件
    public Canvas ZOU_MA_DENG_Canvas;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//获取gameManager实例        
        playercontroller = FindObjectOfType<Player>();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();

        // 启动播放
        videoPlayer.Play();

        // 监听视频播放完成事件
        videoPlayer.loopPointReached += OnVideoFinished;

       
       
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("视频播放完成！");
        // 在这里执行你想要的逻辑，例如隐藏 RawImage 或切换界面
        
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        playercontroller.Canmove = true;
    }

    public void SpeedUp()//加速播放
    {
        videoPlayer.playbackSpeed = 6.0f; // 2 倍速
    }
}
