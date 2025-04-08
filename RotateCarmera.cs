using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("目标设置")]
    public Transform target; // 要跟随的目标（玩家）

    [Header("跟随设置")]
    public float smoothSpeed = 5f; // 平滑跟随的速度
    public bool smoothFollow = true; // 是否使用平滑跟随

    [Header("偏移设置")]
    public Vector3 offset = new Vector3(0, 0, -10); // 相机相对于目标的偏移量
    public bool useRelativeOffset = true; // 如果为true，偏移量会根据相机旋转而改变

    [Header("范围限制")]
    public bool useBounds = false; // 是否限制相机移动范围
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // 如果没有指定目标，尝试找到带有Player标签的游戏对象
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("已自动找到Player目标");
            }
            else
            {
                Debug.LogWarning("未指定跟随目标且场景中没有带Player标签的对象!");
            }
        }

        // 立即设置相机位置到目标位置+偏移，以避免游戏开始时的相机移动
        if (target != null)
        {
            UpdateCameraPosition(false);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        UpdateCameraPosition(smoothFollow);
    }

    void UpdateCameraPosition(bool smooth)
    {
        // 计算目标位置（考虑偏移）
        Vector3 desiredPosition;

        if (useRelativeOffset)
        {
            // 使用相对于相机旋转的偏移
            desiredPosition = target.position + transform.rotation * offset;
        }
        else
        {
            // 使用绝对偏移
            desiredPosition = target.position + offset;
        }

        // 应用边界限制
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        if (smooth)
        {
            // 平滑移动到目标位置
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref velocity,
                1f / smoothSpeed
            );
        }
        else
        {
            // 立即移动到目标位置
            transform.position = desiredPosition;
        }
    }

    // 公共方法：实时调整偏移量
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    // 公共方法：向现有偏移量添加值
    public void AddOffset(Vector3 additionalOffset)
    {
        offset += additionalOffset;
    }

    // 公共方法：重置偏移量到指定的默认值
    public void ResetOffset(Vector3 defaultOffset)
    {
        offset = defaultOffset;
    }

    // 调试绘制
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            // 绘制相机移动的边界线
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
            Vector3 center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, transform.position.z);
            Gizmos.DrawWireCube(center, size);
        }
    }
}