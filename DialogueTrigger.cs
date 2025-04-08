using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DialogueTrigger : MonoBehaviour
{
    private RPGGameManager gameManager; // 声明成员变量
    private Player playercontroller;

    public Rigidbody2D playerRb;

    public Canvas dialogueCanvas; // 要显示的 Canvas
    public Canvas ButtonsCanvas;
    public Image BG;//当前背景

    public Button Button1;
    public Button Button2;
    public Button Button3;

    public TextMeshProUGUI SpeakerContent;

    public Transform OffsetPoint;

    private Collider2D currentCollider; // 存储当前的 Collider
    public string currentStoryFile = null; // 存储当前的故事文件名

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//获取gameManager实例
        playercontroller = FindObjectOfType<Player>();
  
        ButtonsCanvas.gameObject.SetActive(false);

        
        
        
    }

    // 当角色进入触发器时调用
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        playercontroller.Canmove = false;
        // 存储触发的 Collider 和物体名字
        currentCollider = other;
        currentStoryFile = other.gameObject.name;
        
        ButtonsCanvas.gameObject.SetActive(true);

        
        if (gameManager.TAI && gameObject.name == "A1")
        {
            Button1.gameObject.SetActive(false);
        }
        if (gameManager.MIN && gameObject.name == "B2")
        {
            Button1.gameObject.SetActive(false);
        }
        if (gameManager.AN && gameObject.name == "C3")
        {
            Button1.gameObject.SetActive(false);
        }
        Button1.onClick.AddListener(() => LoadStory(currentCollider, currentStoryFile));// 注册谈话
        Button2.onClick.AddListener(Button2_OnClick);
        Button3.onClick.AddListener(Button3_OnClick);
    }

    // 当角色离开触发器时调用
    private void OnTriggerExit2D(Collider2D other)
    {
        // 判断离开触发器的是不是玩家
        if (other.CompareTag("Player"))
        {
            if (playercontroller != null)
            {
                playercontroller.Canmove = true;
                currentCollider = null;
                currentStoryFile = null;
            }
            else
            {
                Debug.Log("jinyongshibai ");
            }
            // 隐藏 dialogueCanvas
            if (dialogueCanvas != null)
            {
                dialogueCanvas.gameObject.SetActive(false);
                ButtonsCanvas.gameObject.SetActive(false);
            }
        }
    }


    private void LoadStory(Collider2D other, string storyFileName)
    {
        
        BG.gameObject.SetActive(true);
        dialogueCanvas.gameObject.SetActive(true);
        ButtonsCanvas.gameObject.SetActive(false);

        

        if (gameObject.name == "A1")    // 使用触发器的名称来判断是哪一个触发器被触发
        {
            HandleTriggerA(other, gameObject.name);
            

        }
        if (gameObject.name == "B2")
        {
            HandleTriggerB(other, gameObject.name);
        }
        if (gameObject.name == "C3")
        {
            HandleTriggerC(other, gameObject.name);
        }
        if (gameObject.name == "D4")
        {
            HandleTriggerD(other, gameObject.name);
        }



    }

    private void HandleTriggerA(Collider2D other, string storyfillname) 
    {
       
        // 判断进入触发器的是不是玩家（确保有 Player 标签）
        if (other.CompareTag("Player") && !gameManager.TAI)
        {

            if (dialogueCanvas != null)
            {

               
                gameManager.StoryChoice(storyfillname);

            }
        }

    }

    // 处理 B 触发器
    private void HandleTriggerB(Collider2D other,string storyfillname)
    {
        // 判断进入触发器的是不是玩家（确保有 Player 标签）
        if (other.CompareTag("Player") && !gameManager.AN)
        {
            // 显示 dialogueCanvas
            if (dialogueCanvas != null)
            {
                gameManager.StoryChoice(storyfillname);
            }
        }
    }

    private void HandleTriggerC(Collider2D other, string storyfillname)
    {

        // 判断进入触发器的是不是玩家（确保有 Player 标签）
        if (other.CompareTag("Player") && !gameManager.MIN)
        {
            if (dialogueCanvas != null)
            {
                
                gameManager.StoryChoice(storyfillname);

            }
        }

    }

    private void HandleTriggerD(Collider2D other, string storyfillname)
    {

        // 判断进入触发器的是不是玩家（确保有 Player 标签）
        if (other.CompareTag("Player") && !gameManager.AN)
        {       
            if (dialogueCanvas != null)
            {
                
                gameManager.StoryChoice(storyfillname);

            }
        }

    }

    private void Initialization_Button()
    {
        TextMeshProUGUI button1Text = Button1.GetComponentInChildren<TextMeshProUGUI>();
        button1Text.text = "对话"; // 设置按钮文本
        TextMeshProUGUI button2Text = Button2.GetComponentInChildren<TextMeshProUGUI>(); // 获取按钮子对象中的Text组件
        button2Text.text = "闲聊"; // 设置按钮文本
        TextMeshProUGUI button3Text = Button3.GetComponentInChildren<TextMeshProUGUI>(); // 获取按钮子对象中的Text组件
        button3Text.text = "退出"; // 设置按钮文本
    }

    private void Button1_OnClick()
    {
       
    }



    private void Button2_OnClick()
    {
        
    }

    private void Button3_OnClick()
    {
        ButtonsCanvas.gameObject.SetActive(false);
        playercontroller.Canmove = true;

        MovePlayerOut();
    }

    public void MovePlayerOut()
    {
        if (OffsetPoint != null)
        {
            Vector2 newPosition = OffsetPoint.position; // 直接获取偏移点的位置
            
            playerRb.MovePosition(newPosition); // 使用 MovePosition 平滑移动
        }
        else
        {
            Debug.LogWarning("偏移点未设置！");
        }
    }

    private void Button4_OnClick()
    {

    }

}
