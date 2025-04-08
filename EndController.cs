using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 需要引入命名空间

public class EndController : MonoBehaviour
{
    private RPGGameManager gameManager; // 声明成员变量
    private Player playercontroller;

    public Canvas EndCanvas;
    public Canvas dialogueCanvas; // 要显示的 Canvas


    // Start is called before the first frame update
    void Start()
    {
        EndCanvas.gameObject.SetActive(false);

        gameManager = FindObjectOfType<RPGGameManager>();//获取gameManager实例
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
            playercontroller.Canmove = false;//禁用移动
            LoadEndStory();
            //EndCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("未达成游戏条件");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }


    public void RestartButtonOnclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重新加载当前场景     
    }

    private void LoadEndStory()
    {
        string fillname = "EndStory";

        dialogueCanvas.gameObject.SetActive(true);
        gameManager.StoryChoice(fillname);



    }

}
