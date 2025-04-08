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
    public string RPG_STORY_FILE_NAME;//故事名
    public readonly string EXCEL_FILE_EXTENSION = ".xlsx";


    private List<EXCELData.RPGExcelData> RPGstoryData;

    private int currentLine = 1;//当前行
    private string currentStoryFileName;//当前故事名
    private string END = "END";//故事结束标志
    private string Endstory = "游戏结束";//标志
    private string Game1 = "汤圆";

    public TextMeshProUGUI ThisText;//当前对话界面
    public TextMeshProUGUI ThisName;//当前对话名称
    public Image ThisImage;//当前对话头像
    public Image ThisBG;//当前背景


    [SerializeField] public bool GUO = false; //是否获取"国"
    [SerializeField] public bool TAI = false;        
    [SerializeField] public bool MIN = false;
    [SerializeField] public bool AN = false;

    public Canvas YUAN_XIAO_Canvas;//元宵游戏画布
    public Canvas ZOU_MA_DENG_Canvas;
    public Canvas TAN_QIN_Canvas;
    public Canvas DA_TE_HUA_Canvas;
    public Canvas ENDSTORY_Canvas;//结束画布

    public Canvas LIAO_TIAN_Canvas;//聊天画布
    public Canvas START_GAME_Canvas;//开始画布



    public Button ChoiceButton1;//分支选择按钮
    public Button ChoiceButton2;
    public Button ChoiceButton3;
    public Button YUAN_XIAO_Button;//进入元宵游戏按钮
    public Button EXIT_BUTTON;//退出按钮


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
            DisplayNextLine();//显示对话框中的下一行文本           
        }
    }


    void Initalize()//初始化界面
    {
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        TAN_QIN_Canvas.gameObject.SetActive(false);
        DA_TE_HUA_Canvas.gameObject.SetActive(false);
        LIAO_TIAN_Canvas.gameObject.SetActive(false);
        YUAN_XIAO_Canvas.gameObject.SetActive(false);
        ZOU_MA_DENG_Canvas.gameObject.SetActive(false);
        TAN_QIN_Canvas.gameObject.SetActive(false);
        player = FindObjectOfType<Player>();//获取Player实例

        

    }



    void bottomButtonsAddListener()//为按钮添加监听器
    {
        YUAN_XIAO_Button.onClick.AddListener(OnYUAN_XIAO_ButtonClick);
        EXIT_BUTTON.onClick.AddListener(OnEXIT_BUTTONClick);
        ChoiceButton1.onClick.AddListener(ChoiceButton1_OnClick);
        ChoiceButton2.onClick.AddListener(ChoiceButton2_OnClick);
        ChoiceButton3.onClick.AddListener(ChoiceButton3_OnClick);



    }

    #endregion

    private void LoadStartStory()//加载开始故事
    {
        ThisBG.gameObject.SetActive(false);
        


        SetChoiceButtonClose();

        StoryChoice("StartStory");
        LoadText("StartStory");
        
        player.Canmove = false;

    }

    

    public void StoryChoice(string StoryNumber)//故事选择
    {
        SetChoiceButtonClose();
        if (StoryNumber != null)
        {
            RPG_STORY_FILE_NAME = StoryNumber;
            LoadText(StoryNumber);
        }
        else
        {
            Debug.Log("故事号码为空");
        }

    }


    private void LoadText(string fillname)//加载故事
    {
        currentStoryFileName = fillname;//为当前故事名赋值
        currentLine = 1;//重置故事当前行

        if (fillname != null)
        {
            var path = RPG_STORY_PATH + fillname + EXCEL_FILE_EXTENSION;
            RPGstoryData = EXCELData.ReadRPGExcel(path);
            ThisLine();
        }
        else
        {
            Debug.Log("故事名为空");
        }


        if (RPGstoryData == null || RPGstoryData.Count == 0)//如果data等于空或data行数为0
        {
            Debug.LogError("No data found");
        }
        


    }


    void DisplayNextLine()//展示下一行
    {
        if (currentLine <= RPGstoryData.Count-1)
        {
            if (RPGstoryData[currentLine].RPGspeakingContent == Game1)//检测故事1结束
            {
                LIAO_TIAN_Canvas.gameObject.SetActive(false);
                YUAN_XIAO_Canvas.gameObject.SetActive(true);
                
            }
            if (RPGstoryData[currentLine].RPGspeakingContent == END)//检测故事结束
            {
                CheckStoryEnd();
                LIAO_TIAN_Canvas.gameObject.SetActive(false);
                
                
            }
            if (RPGstoryData[currentLine].RPGspeakingContent == Endstory)//检测到游戏结束
            {
                ENDSTORY_Canvas.gameObject.SetActive(true);

                //SetChoiceButtonOpen();//打开分支按钮
            }
            else
            {
                ThisLine();
            }
        }
        
    }


    public void ThisLine()//展示当前故事行
    {

        var rpgdata = RPGstoryData[currentLine];

        ThisName.text = rpgdata.RPGspeakerName;//当前说话人名称   
        ThisText.text = rpgdata.RPGspeakingContent;//当前打字内容

        if (NotNullNorEmpty(rpgdata.RPGBackground))
        {
            UpdateBackgroundImage(rpgdata.RPGBackground);//传入图片名称
        }
        currentLine++;//下一行
    }

    bool NotNullNorEmpty(string str)//检查字符串是否为空或长度为0
    {
        return !string.IsNullOrEmpty(str);
    }




    void UpdateBackgroundImage(string imageFileName)//更新背景图片
    {
        string imagePath = RPG_STORY_BG + imageFileName;//图片路径+图片名称
        UpdateImage(imagePath, ThisBG);
    }

    void UpdateImage(string imagePath, Image image)//更新图像
    {
        Sprite sprite = Resources.Load<Sprite>(imagePath);//从Resources加载图像
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
            Debug.Log("获得泰");
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
            Debug.Log("获得国");
            GUO = true;
            player.Canmove = true;
        }
    }






    #region Button
    private void OnYUAN_XIAO_ButtonClick() // 进入游戏按钮
    {
        
    }

    private void OnEXIT_BUTTONClick()//退出 button
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
            Debug.Log("未找到B2");
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
        StoryChoice(A1);//加载A1故事
        LIAO_TIAN_Canvas.gameObject.SetActive(true);
    }


    












}
