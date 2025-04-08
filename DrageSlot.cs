using UnityEngine;
using UnityEngine.EventSystems; // 引入事件系统，用于处理拖放事件
using System.Collections;
using static UnityEngine.GraphicsBuffer;

// 实现IDropHandler接口，该接口用于检测UI元素被拖拽到该对象上时的行为
public class DrageSlot : MonoBehaviour, IDropHandler
{
    public bool isFinish;
    
    // 当拖拽的UI元素放置到这个对象上时触发
    public void OnDrop(PointerEventData eventData)
    {
        // 获取被拖拽的物体（eventData.pointerDrag）并将其 RectTransform 组件的锚定位置
        // 设置为当前物体的 RectTransform 组件的锚定位置，使其对齐
        RectTransform draggedObject = eventData.pointerDrag.GetComponent<RectTransform>();
        //开始平滑移动物体
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

        obj.anchoredPosition = targetPos; // 确保最终对齐

        //  **正确放置后，设置 isFinish**
        DragByEven dragScript = obj.GetComponent<DragByEven>();
        if (dragScript != null && targetPos == GetComponent<RectTransform>().anchoredPosition)
        {
            dragScript.isFinish = true;
        }

    }

}
