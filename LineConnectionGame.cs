using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LineConnectionGame : MonoBehaviour
{
    // �����л��ֶΣ�������Unity Inspector������
    [SerializeField] private List<Image> connectionTargets;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Color lineColor = Color.blue;
    [SerializeField] private float linethickness = 5f;

    // ��ȷ������˳��
    [SerializeField] private List<int> correctConnectionOrder;

    // ��ǰ����״̬
    private List<int> currentConnectionOrder = new List<int>();
    private Image currentStartImage = null;
    private LineRenderer currentLine = null;
    private float lineThickness;

    void Start()
    {
        // Ϊÿ��ͼ����ӵ���¼�������
        for (int i = 0; i < connectionTargets.Count; i++)
        {
            int index = i;
            connectionTargets[i].GetComponent<Button>().onClick.AddListener(() => OnImageClicked(connectionTargets[index]));
        }
    }

    void OnImageClicked(Image clickedImage)
    {
        // ����ǵ�һ�ε������Լ�������
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
        // �����߶�
        GameObject lineObject = Instantiate(linePrefab, transform);
        currentLine = lineObject.GetComponent<LineRenderer>();

        // �����߶���ʽ
        currentLine.startColor = lineColor;
        currentLine.endColor = lineColor;
        currentLine.startWidth = lineThickness;
        currentLine.endWidth = lineThickness;

        // �������
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, startImage.rectTransform.position);
        currentLine.SetPosition(1, startImage.rectTransform.position);

        currentStartImage = startImage;
    }

    void ConnectToImage(Image endImage)
    {
        // ��ֹ�ظ����Ӻͻ���
        if (endImage == currentStartImage ||
            (currentConnectionOrder.Count > 0 &&
             connectionTargets.IndexOf(endImage) == currentConnectionOrder[currentConnectionOrder.Count - 1]))
        {
            return;
        }

        // �����߶��յ�
        currentLine.SetPosition(1, endImage.rectTransform.position);

        // ��¼����˳��
        int endImageIndex = connectionTargets.IndexOf(endImage);
        currentConnectionOrder.Add(endImageIndex);

        // ����Ƿ�ƥ����ȷ˳��
        if (CheckConnectionOrder())
        {
            Debug.Log("���ӳɹ���");
        }

        // ׼����һ����
        currentStartImage = endImage;
        StartNewLine(endImage);
    }

    bool CheckConnectionOrder()
    {
        // �Ƚϵ�ǰ����˳���Ԥ�������ȷ˳��
        if (currentConnectionOrder.Count != correctConnectionOrder.Count)
            return false;

        for (int i = 0; i < currentConnectionOrder.Count; i++)
        {
            if (currentConnectionOrder[i] != correctConnectionOrder[i])
                return false;
        }

        return true;
    }

    // ������Ϸ����
    public void ResetGame()
    {
        // ���������Ѵ�������
        foreach (LineRenderer line in FindObjectsOfType<LineRenderer>())
        {
            Destroy(line.gameObject);
        }

        currentConnectionOrder.Clear();
        currentStartImage = null;
        currentLine = null;
    }
}