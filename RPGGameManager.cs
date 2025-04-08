using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using DG.Tweening;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class RPGGameManager : MonoBehaviour
{
    #region Variable
    //private readonly string RPGstoryPath = "Assets/RPGResources/Text/A1.xlsx";
    public readonly string RPG_STORY_PATH = "Assets/RPGResources/Text/";
    public readonly string RPG_STORY_BG = "RPGBG/";
    public string RPG_STORY_FILE_NAME;//������
    public readonly string EXCEL_FILE_EXTENSION = ".xlsx";


    private List<EXCELData.RPGExcelData> RPGstoryData;

    private int currentLine = 1;//��ǰ��
    private string currentStoryFileName;//��ǰ������
    private string END = "END";//���½�����־
    private string Endstory = "��Ϸ����";//��־
    private string Game1 = "��Բ";

    public TextMeshProUGUI ThisText;//��ǰ�Ի�����
    public TextMeshProUGUI ThisName;//��ǰ�Ի�����
    public Image ThisImage;//��ǰ�Ի�ͷ��
    public Image ThisBG;//��ǰ����


    [SerializeField] public bool GUO = false; //�Ƿ��ȡ"��"
    [SerializeField] public bool TAI = false;        
    [SerializeField] public bool MIN = false;
    [SerializeField] public bool AN = false;

    public Canvas YUAN_XIAO_Canvas;//Ԫ����Ϸ����
    public Canvas ZOU_MA_DENG_Canvas;
    public Canvas TAN_QIN_Canvas;
    public Canvas DA_TE_HUA_Canvas;
    public Canvas ENDSTORY_Canvas;//��������

    public Canvas LIAO_TIAN_Canvas;//���컭��
    public Canvas START_GAME_Canvas;//��ʼ����



    public Button ChoiceButton1;//��֧ѡ��ť
    public Button ChoiceButton2;
    public Button ChoiceButton3;
    public Button YUAN_XIAO_Button;//����Ԫ����Ϸ��ť
    public Button EXIT_BUTTON;//�˳���ť


    public Player player;
    private DialogueTrigger DialogueTrigger;
    public Transform OffsetPoint;
    

    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        Initalize();
        LoadStartStory();
        bottomButtonsAddListener();      
  
    }

    // Update is called once per frame
    void Update()
    {
        if (LIAO_TIAN_Canvas.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();//��ʾ�Ի����е���һ���ı�           
        }
    }


    void Initalize()//��ʼ������
    {
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        TAN_QIN_Canvas.gameObject.SetActive(false);
        DA_TE_HUA_Canvas.gameObject.SetActive(false);
        LIAO_TIAN_Canvas.gameObject.SetActive(false);
        YUAN_XIAO_Canvas.gameObject.SetActive(false);
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        TAN_QIN_Canvas.gameObject.SetActive(false);
        player = FindObjectOfType<Player>();//��ȡPlayerʵ��

        

    }



    void bottomButtonsAddListener()//Ϊ��ť��Ӽ�����
    {
        YUAN_XIAO_Button.onClick.AddListener(OnYUAN_XIAO_ButtonClick);
        EXIT_BUTTON.onClick.AddListener(OnEXIT_BUTTONClick);
        ChoiceButton1.onClick.AddListener(ChoiceButton1_OnClick);
        ChoiceButton2.onClick.AddListener(ChoiceButton2_OnClick);
        ChoiceButton3.onClick.AddListener(ChoiceButton3_OnClick);



    }

    #endregion

    private void LoadStartStory()//���ؿ�ʼ����
    {
        ThisBG.gameObject.SetActive(false);
        


        SetChoiceButtonClose();

        StoryChoice("StartStory");
        LoadText("StartStory");
        
        player.Canmove = false;

    }

    

    public void StoryChoice(string StoryNumber)//����ѡ��
    {
        SetChoiceButtonClose();
        if (StoryNumber != null)
        {
            RPG_STORY_FILE_NAME = StoryNumber;
            LoadText(StoryNumber);
        }
        else
        {
            Debug.Log("���º���Ϊ��");
        }

    }


    private void LoadText(string fillname)//���ع���
    {
        currentStoryFileName = fillname;//Ϊ��ǰ��������ֵ
        currentLine = 1;//���ù��µ�ǰ��

        if (fillname != null)
        {
            var path = RPG_STORY_PATH + fillname + EXCEL_FILE_EXTENSION;
            RPGstoryData = EXCELData.ReadRPGExcel(path);
            ThisLine();
        }
        else
        {
            Debug.Log("������Ϊ��");
        }


        if (RPGstoryData == null || RPGstoryData.Count == 0)//���data���ڿջ�data����Ϊ0
        {
            Debug.LogError("No data found");
        }
        


    }


    void DisplayNextLine()//չʾ��һ��
    {
        if (currentLine <= RPGstoryData.Count-1)
        {
            if (RPGstoryData[currentLine].RPGspeakingContent == Game1)//������1����
            {
                LIAO_TIAN_Canvas.gameObject.SetActive(false);
                YUAN_XIAO_Canvas.gameObject.SetActive(true);
                
            }
            if (RPGstoryData[currentLine].RPGspeakingContent == END)//�����½���
            {
                CheckStoryEnd();
                LIAO_TIAN_Canvas.gameObject.SetActive(false);
                
                
            }
            if (RPGstoryData[currentLine].RPGspeakingContent == Endstory)//��⵽��Ϸ����
            {
                ENDSTORY_Canvas.gameObject.SetActive(true);

                //SetChoiceButtonOpen();//�򿪷�֧��ť
            }
            else
            {
                ThisLine();
            }
        }
        
    }


    public void ThisLine()//չʾ��ǰ������
    {

        var rpgdata = RPGstoryData[currentLine];

        ThisName.text = rpgdata.RPGspeakerName;//��ǰ˵��������   
        ThisText.text = rpgdata.RPGspeakingContent;//��ǰ��������

        if (NotNullNorEmpty(rpgdata.RPGBackground))
        {
            UpdateBackgroundImage(rpgdata.RPGBackground);//����ͼƬ����
        }
        currentLine++;//��һ��
    }

    bool NotNullNorEmpty(string str)//����ַ����Ƿ�Ϊ�ջ򳤶�Ϊ0
    {
        return !string.IsNullOrEmpty(str);
    }




    void UpdateBackgroundImage(string imageFileName)//���±���ͼƬ
    {
        string imagePath = RPG_STORY_BG + imageFileName;//ͼƬ·��+ͼƬ����
        UpdateImage(imagePath, ThisBG);
    }

    void UpdateImage(string imagePath, Image image)//����ͼ��
    {
        Sprite sprite = Resources.Load<Sprite>(imagePath);//��Resources����ͼ��
        if (sprite != null)
        {
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(imagePath);
        }
    }

    private void CheckStoryEnd()
    {            
        if (RPG_STORY_FILE_NAME == "A1")    
        {
            Debug.Log("���̩");
            TAI = true;
            ZOU_MA_DENG_Canvas.gameObject.SetActive(true);
        }
        if (RPG_STORY_FILE_NAME == "B2")
        {
            
        }
        if (RPG_STORY_FILE_NAME == "C3")
        {
            
        }
        if (RPG_STORY_FILE_NAME == "tangyuan-mid")
        {
            Debug.Log("��ù�");
            GUO = true;
            player.Canmove = true;
        }
    }






    #region Button
    private void OnYUAN_XIAO_ButtonClick() // ������Ϸ��ť
    {
        
    }

    private void OnEXIT_BUTTONClick()//�˳� button
    {
        if (OffsetPoint != null)
        {
            DialogueTrigger.MovePlayerOut();
        }     
        YUAN_XIAO_Canvas.gameObject.SetActive(false);
        LIAO_TIAN_Canvas.gameObject.SetActive(false);
        

        player.Canmove = true;
        

    }


    #region ChiceButton

    private void SetChoiceButtonClose()
    {        
        ChoiceButton1.gameObject.SetActive(false);
        ChoiceButton2.gameObject.SetActive(false);
        ChoiceButton3.gameObject.SetActive(false);
    }
    private void SetChoiceButtonOpen()
    {
        
        
        if (currentStoryFileName == "B2")
        {
            ZOU_MA_DENG_Canvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("δ�ҵ�B2");
        }
        

    }

    private void ChoiceButton1_OnClick()
    {
        YUAN_XIAO_Canvas.gameObject.SetActive(true);
        LIAO_TIAN_Canvas.gameObject.SetActive(false);
        
    }

    private void ChoiceButton2_OnClick()
    {
        ThisLine();
    }
    private void ChoiceButton3_OnClick()
    {
        ThisLine();
    }

    #endregion
    #endregion



    public void tangyuanCanvasOpen()
    {
        string A1 = "A1";
        StoryChoice(A1);//����A1����
        LIAO_TIAN_Canvas.gameObject.SetActive(true);
    }


    












}
