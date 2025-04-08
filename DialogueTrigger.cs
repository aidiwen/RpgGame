using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DialogueTrigger : MonoBehaviour
{
    private RPGGameManager gameManager; // ������Ա����
    private Player playercontroller;

    public Rigidbody2D playerRb;

    public Canvas dialogueCanvas; // Ҫ��ʾ�� Canvas
    public Canvas ButtonsCanvas;
    public Image BG;//��ǰ����

    public Button Button1;
    public Button Button2;
    public Button Button3;

    public TextMeshProUGUI SpeakerContent;

    public Transform OffsetPoint;

    private Collider2D currentCollider; // �洢��ǰ�� Collider
    public string currentStoryFile = null; // �洢��ǰ�Ĺ����ļ���

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<RPGGameManager>();//��ȡgameManagerʵ��
        playercontroller = FindObjectOfType<Player>();
  
        ButtonsCanvas.gameObject.SetActive(false);

        
        
        
    }

    // ����ɫ���봥����ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        playercontroller.Canmove = false;
        // �洢������ Collider ����������
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
        Button1.onClick.AddListener(() => LoadStory(currentCollider, currentStoryFile));// ע��̸��
        Button2.onClick.AddListener(Button2_OnClick);
        Button3.onClick.AddListener(Button3_OnClick);
    }

    // ����ɫ�뿪������ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        // �ж��뿪���������ǲ������
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
            // ���� dialogueCanvas
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

        

        if (gameObject.name == "A1")    // ʹ�ô��������������ж�����һ��������������
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
       
        // �жϽ��봥�������ǲ�����ң�ȷ���� Player ��ǩ��
        if (other.CompareTag("Player") && !gameManager.TAI)
        {

            if (dialogueCanvas != null)
            {

               
                gameManager.StoryChoice(storyfillname);

            }
        }

    }

    // ���� B ������
    private void HandleTriggerB(Collider2D other,string storyfillname)
    {
        // �жϽ��봥�������ǲ�����ң�ȷ���� Player ��ǩ��
        if (other.CompareTag("Player") && !gameManager.AN)
        {
            // ��ʾ dialogueCanvas
            if (dialogueCanvas != null)
            {
                gameManager.StoryChoice(storyfillname);
            }
        }
    }

    private void HandleTriggerC(Collider2D other, string storyfillname)
    {

        // �жϽ��봥�������ǲ�����ң�ȷ���� Player ��ǩ��
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

        // �жϽ��봥�������ǲ�����ң�ȷ���� Player ��ǩ��
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
        button1Text.text = "�Ի�"; // ���ð�ť�ı�
        TextMeshProUGUI button2Text = Button2.GetComponentInChildren<TextMeshProUGUI>(); // ��ȡ��ť�Ӷ����е�Text���
        button2Text.text = "����"; // ���ð�ť�ı�
        TextMeshProUGUI button3Text = Button3.GetComponentInChildren<TextMeshProUGUI>(); // ��ȡ��ť�Ӷ����е�Text���
        button3Text.text = "�˳�"; // ���ð�ť�ı�
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
            Vector2 newPosition = OffsetPoint.position; // ֱ�ӻ�ȡƫ�Ƶ��λ��
            
            playerRb.MovePosition(newPosition); // ʹ�� MovePosition ƽ���ƶ�
        }
        else
        {
            Debug.LogWarning("ƫ�Ƶ�δ���ã�");
        }
    }

    private void Button4_OnClick()
    {

    }

}
