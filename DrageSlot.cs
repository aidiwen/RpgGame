using UnityEngine;
using UnityEngine.EventSystems; // �����¼�ϵͳ�����ڴ����Ϸ��¼�
using System.Collections;
using static UnityEngine.GraphicsBuffer;

// ʵ��IDropHandler�ӿڣ��ýӿ����ڼ��UIԪ�ر���ק���ö�����ʱ����Ϊ
public class DrageSlot : MonoBehaviour, IDropHandler
{
    public bool isFinish;
    
    // ����ק��UIԪ�ط��õ����������ʱ����
    public void OnDrop(PointerEventData eventData)
    {
        // ��ȡ����ק�����壨eventData.pointerDrag�������� RectTransform �����ê��λ��
        // ����Ϊ��ǰ����� RectTransform �����ê��λ�ã�ʹ�����
        RectTransform draggedObject = eventData.pointerDrag.GetComponent<RectTransform>();
        //��ʼƽ���ƶ�����
        StartCoroutine(SmoothMove(draggedObject, GetComponent<RectTransform>().anchoredPosition, 0.2f));
       


    }


    private IEnumerator SmoothMove(RectTransform obj, Vector2 targetPos, float duration)
    {
        Vector2 startPos = obj.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            obj.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        obj.anchoredPosition = targetPos; // ȷ�����ն���

        //  **��ȷ���ú����� isFinish**
        DragByEven dragScript = obj.GetComponent<DragByEven>();
        if (dragScript != null && targetPos == GetComponent<RectTransform>().anchoredPosition)
        {
            dragScript.isFinish = true;
        }

    }

}
