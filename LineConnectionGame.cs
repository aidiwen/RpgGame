using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LineConnectionGame : MonoBehaviour
{
    // 可序列化字段，方便在Unity Inspector中配置
    [SerializeField] private List<Image> connectionTargets;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Color lineColor = Color.blue;
    [SerializeField] private float linethickness = 5f;

    // 正确的连接顺序
    [SerializeField] private List<int> correctConnectionOrder;

    // 当前连接状态
    private List<int> currentConnectionOrder = new List<int>();
    private Image currentStartImage = null;
    private LineRenderer currentLine = null;
    private float lineThickness;

    void Start()
    {
        // 为每个图像添加点击事件监听器
        for (int i = 0; i < connectionTargets.Count; i++)
        {
            int index = i;
            connectionTargets[i].GetComponent<Button>().onClick.AddListener(() => OnImageClicked(connectionTargets[index]));
        }
    }

    void OnImageClicked(Image clickedImage)
    {
        // 如果是第一次点击或可以继续连线
        if (currentStartImage == null)
        {
            StartNewLine(clickedImage);
        }
        else
        {
            ConnectToImage(clickedImage);
        }
    }

    void StartNewLine(Image startImage)
    {
        // 创建线段
        GameObject lineObject = Instantiate(linePrefab, transform);
        currentLine = lineObject.GetComponent<LineRenderer>();

        // 配置线段样式
        currentLine.startColor = lineColor;
        currentLine.endColor = lineColor;
        currentLine.startWidth = lineThickness;
        currentLine.endWidth = lineThickness;

        // 设置起点
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, startImage.rectTransform.position);
        currentLine.SetPosition(1, startImage.rectTransform.position);

        currentStartImage = startImage;
    }

    void ConnectToImage(Image endImage)
    {
        // 防止重复连接和回连
        if (endImage == currentStartImage ||
            (currentConnectionOrder.Count > 0 &&
             connectionTargets.IndexOf(endImage) == currentConnectionOrder[currentConnectionOrder.Count - 1]))
        {
            return;
        }

        // 更新线段终点
        currentLine.SetPosition(1, endImage.rectTransform.position);

        // 记录连接顺序
        int endImageIndex = connectionTargets.IndexOf(endImage);
        currentConnectionOrder.Add(endImageIndex);

        // 检查是否匹配正确顺序
        if (CheckConnectionOrder())
        {
            Debug.Log("连接成功！");
        }

        // 准备下一条线
        currentStartImage = endImage;
        StartNewLine(endImage);
    }

    bool CheckConnectionOrder()
    {
        // 比较当前连接顺序和预定义的正确顺序
        if (currentConnectionOrder.Count != correctConnectionOrder.Count)
            return false;

        for (int i = 0; i < currentConnectionOrder.Count; i++)
        {
            if (currentConnectionOrder[i] != correctConnectionOrder[i])
                return false;
        }

        return true;
    }

    // 重置游戏方法
    public void ResetGame()
    {
        // 销毁所有已创建的线
        foreach (LineRenderer line in FindObjectsOfType<LineRenderer>())
        {
            Destroy(line.gameObject);
        }

        currentConnectionOrder.Clear();
        currentStartImage = null;
        currentLine = null;
    }
}