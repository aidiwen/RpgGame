using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;

public class DragController : MonoBehaviour
{
    private RPGGameManager gameManager; // ������Ա����
    [SerializeField] private List<DragByEven> draggableItems;

    private bool firstCompleted = false;  // ��¼��һ���Ƿ����
    private bool secondCompleted = false; // ��¼�ڶ����Ƿ����
    private bool ThirdCompleted = false; //��¼�������Ƿ����

    public Canvas dialogueCanvas; // ���� Canvas
    public Canvas tangyuanCanvas;

    private int currentIndex = 0; // ��¼��ǰ���õ���������

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


    [SerializeField] private GameObject panel1; // ��ק��ϷPanel
    [SerializeField] private GameObject panel2; // ͼƬ�л�Panel
    [SerializeField] private List<Image> imageList; // ͼƬ�б�
    private int currentImageIndex = 0; // ��ǰ��ʾ��ͼƬ����




    private void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//��ȡgameManagerʵ��
        bowl_water.gameObject.SetActive(false);
        bowl2.gameObject.SetActive(false);
        top_3_1.gameObject.SetActive(false);
        top_3_2.gameObject.SetActive(false);
        top_end.gameObject.SetActive(false);
        bowl_tangyuan.gameObject.SetActive(false);
        panel2.gameObject.SetActive(false); 
        Win.gameObject.SetActive(false);
        bowl_tangyuan_end.gameObject .SetActive(false);


        draggableItems[1].enabled = false; // ����2���������ק
        draggableItems[2].enabled = false;
    }

    public void EnableNextDraggable()
    {
        currentIndex++;

        if (currentIndex < draggableItems.Count)
        {
            draggableItems[currentIndex].gameObject.SetActive(true); // ������һ���϶�����
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
                    draggableItems[1].enabled = true; // ����2���������ק
                    
                    // �������ִ�� 1 ��ɺ���߼�
                }

                if (i == 1 && !secondCompleted)
                {
                    secondCompleted = true;
                    top_3_1.gameObject.SetActive(false);
                    top_3_2.gameObject.SetActive(true);
                    bowl_tangyuan.gameObject.SetActive(true);
                    draggableItems[2].enabled = true;
                   
                    // �������ִ�� 2 ��ɺ���߼�
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
