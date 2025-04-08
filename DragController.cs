using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;

public class DragController : MonoBehaviour
{
    private RPGGameManager gameManager; // 声明成员变量
    [SerializeField] private List<DragByEven> draggableItems;

    private bool firstCompleted = false;  // 记录第一个是否完成
    private bool secondCompleted = false; // 记录第二个是否完成
    private bool ThirdCompleted = false; //记录第三个是否完成

    public Canvas dialogueCanvas; // 聊天 Canvas
    public Canvas tangyuanCanvas;

    private int currentIndex = 0; // 记录当前启用的物体索引

    public Image bowl1;
    public Image bowl2;
    public Image bowl_water;
    public Image bowl_nowater;
    public Image top_3_1;
    public Image top_3_2;
    public Image top_end;
    public Image bowl_tangyuan;    
    public Image bowl_tangyuan_end;
    public Image Win;


    [SerializeField] private GameObject panel1; // 拖拽游戏Panel
    [SerializeField] private GameObject panel2; // 图片切换Panel
    [SerializeField] private List<Image> imageList; // 图片列表
    private int currentImageIndex = 0; // 当前显示的图片索引




    private void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//获取gameManager实例
        bowl_water.gameObject.SetActive(false);
        bowl2.gameObject.SetActive(false);
        top_3_1.gameObject.SetActive(false);
        top_3_2.gameObject.SetActive(false);
        top_end.gameObject.SetActive(false);
        bowl_tangyuan.gameObject.SetActive(false);
        panel2.gameObject.SetActive(false); 
        Win.gameObject.SetActive(false);
        bowl_tangyuan_end.gameObject .SetActive(false);


        draggableItems[1].enabled = false; // 禁用2号物体的拖拽
        draggableItems[2].enabled = false;
    }

    public void EnableNextDraggable()
    {
        currentIndex++;

        if (currentIndex < draggableItems.Count)
        {
            draggableItems[currentIndex].gameObject.SetActive(true); // 启用下一个拖动物体
        }
    }

    public void CheckAllFinished()
    {
        bool allFinished = true;
        

        for (int i = 0; i < draggableItems.Count; i++)
        {
            if (!draggableItems[i].isFinish)
            {
                allFinished = false;
            }
            else
            {
                if (i == 0 && !firstCompleted)
                {
                    firstCompleted = true;
                    bowl_water.gameObject.SetActive(true);
                    bowl2.gameObject.SetActive(true);
                    bowl1.gameObject.SetActive(false);
                    bowl_nowater.gameObject.SetActive(false);
                    top_3_1.gameObject.SetActive(true);
                    draggableItems[1].enabled = true; // 启用2号物体的拖拽
                    
                    // 这里可以执行 1 完成后的逻辑
                }

                if (i == 1 && !secondCompleted)
                {
                    secondCompleted = true;
                    top_3_1.gameObject.SetActive(false);
                    top_3_2.gameObject.SetActive(true);
                    bowl_tangyuan.gameObject.SetActive(true);
                    draggableItems[2].enabled = true;
                   
                    // 这里可以执行 2 完成后的逻辑
                }
                if (i == 2 && !ThirdCompleted)
                {
                    top_3_2.gameObject.SetActive(false);
                    top_end.gameObject.SetActive(true);
                    bowl_tangyuan_end.gameObject.SetActive(true);

                    



                }
            }
        }

        if (allFinished)
        {
            ExecuteNextStep();
        }
    }

    private void ExecuteNextStep()
    {       
        Showbowl_tangyuan_endOnclick();
       
        
    }

    public void Showbowl_tangyuan_endOnclick()
    {
        Win.gameObject.SetActive(true);
    }

    public void WinButtonOnclick()
    {
        string tangyuan = "tangyuan-mid";

        tangyuanCanvas.gameObject.SetActive(false);
        dialogueCanvas.gameObject.SetActive(true);
        gameManager.StoryChoice(tangyuan);

    }

    
}
