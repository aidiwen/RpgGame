using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Ŀ������")]
    public Transform target; // Ҫ�����Ŀ�꣨��ң�

    [Header("��������")]
    public float smoothSpeed = 5f; // ƽ��������ٶ�
    public bool smoothFollow = true; // �Ƿ�ʹ��ƽ������

    [Header("ƫ������")]
    public Vector3 offset = new Vector3(0, 0, -10); // ��������Ŀ���ƫ����
    public bool useRelativeOffset = true; // ���Ϊtrue��ƫ��������������ת���ı�

    [Header("��Χ����")]
    public bool useBounds = false; // �Ƿ���������ƶ���Χ
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // ���û��ָ��Ŀ�꣬�����ҵ�����Player��ǩ����Ϸ����
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("���Զ��ҵ�PlayerĿ��");
            }
            else
            {
                Debug.LogWarning("δָ������Ŀ���ҳ�����û�д�Player��ǩ�Ķ���!");
            }
        }

        // �����������λ�õ�Ŀ��λ��+ƫ�ƣ��Ա�����Ϸ��ʼʱ������ƶ�
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
        // ����Ŀ��λ�ã�����ƫ�ƣ�
        Vector3 desiredPosition;

        if (useRelativeOffset)
        {
            // ʹ������������ת��ƫ��
            desiredPosition = target.position + transform.rotation * offset;
        }
        else
        {
            // ʹ�þ���ƫ��
            desiredPosition = target.position + offset;
        }

        // Ӧ�ñ߽�����
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        if (smooth)
        {
            // ƽ���ƶ���Ŀ��λ��
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref velocity,
                1f / smoothSpeed
            );
        }
        else
        {
            // �����ƶ���Ŀ��λ��
            transform.position = desiredPosition;
        }
    }

    // ����������ʵʱ����ƫ����
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    // ����������������ƫ�������ֵ
    public void AddOffset(Vector3 additionalOffset)
    {
        offset += additionalOffset;
    }

    // ��������������ƫ������ָ����Ĭ��ֵ
    public void ResetOffset(Vector3 defaultOffset)
    {
        offset = defaultOffset;
    }

    // ���Ի���
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            // ��������ƶ��ı߽���
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
            Vector3 center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, transform.position.z);
            Gizmos.DrawWireCube(center, size);
        }
    }
}