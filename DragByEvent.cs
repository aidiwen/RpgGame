using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragByEven : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTrans;
    private CanvasGroup canvasGroup;
    private DragController dragController; // 引用控制器

    public bool isFinish = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float snapThreshold = 50f;
    [SerializeField] private float returnSpeed = 5f;
    private Vector2 originalPosition;
    [SerializeField] private RectTransform correctTargetPoint;

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalPosition = rectTrans.anchoredPosition;

        // 获取 DragController 实例
        dragController = FindObjectOfType<DragController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isFinish)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isFinish)
        {
            rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isFinish)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            float distance = Vector2.Distance(rectTrans.anchoredPosition, correctTargetPoint.anchoredPosition);
            if (distance < snapThreshold)
            {
                StartCoroutine(SmoothMove(correctTargetPoint.anchoredPosition, true));
            }
            else
            {
                StartCoroutine(SmoothMove(originalPosition, false));
            }
        }
    }

    private IEnumerator SmoothMove(Vector2 target, bool lockDrag)
    {
        Vector2 start = rectTrans.anchoredPosition;
        float duration = 0.3f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            rectTrans.anchoredPosition = Vector2.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTrans.anchoredPosition = target;

        if (lockDrag)
        {
            isFinish = true;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false); // 禁用当前物体
            dragController.EnableNextDraggable(); // 启用下一个拖动物体
        }
        else
        {
            isFinish = false;
            canvasGroup.blocksRaycasts = true;
        }

        dragController.CheckAllFinished();
    }
}
