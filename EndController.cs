using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ��Ҫ���������ռ�

public class EndController : MonoBehaviour
{
    private RPGGameManager gameManager; // ������Ա����
    private Player playercontroller;

    public Canvas EndCanvas;
    public Canvas dialogueCanvas; // Ҫ��ʾ�� Canvas


    // Start is called before the first frame update
    void Start()
    {
        EndCanvas.gameObject.SetActive(false);

        gameManager = FindObjectOfType<RPGGameManager>();//��ȡgameManagerʵ��
        playercontroller = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager.GUO && gameManager.TAI /*&& gameManager.MIN && gameManager.AN*/)
        {
            playercontroller.Canmove = false;//�����ƶ�
            LoadEndStory();
            //EndCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("δ�����Ϸ����");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }


    public void RestartButtonOnclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���¼��ص�ǰ����     
    }

    private void LoadEndStory()
    {
        string fillname = "EndStory";

        dialogueCanvas.gameObject.SetActive(true);
        gameManager.StoryChoice(fillname);



    }

}
