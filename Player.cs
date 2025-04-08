using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("��ɫ�ٶ�")]
    public float speed;
    public float clickMoveSpeed = 5f; // Speed for click-to-move
    public float stoppingDistance = 0.1f; // How close to get to target before stopping

    public bool Canmove = true;

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private float inputX, inputY;
    private Vector3 offset;

    // Variables for mouse movement
    
    private bool isMovingToClickPosition = false;
    private Vector2 targetPosition;

    void Start()
    {
        offset = Camera.main.transform.position - transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (Canmove)
            {
                // �޸������������ת�������λ�ü���
                Vector3 mouseWorldPos = GetMouseWorldPosition();
                targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                /* �������
                Debug.Log("�����ת�Ƕ�: " + Camera.main.transform.eulerAngles.z +
                         ", Ŀ��λ��: " + targetPosition +
                         ", ��ǰλ��: " + transform.position);
                */
                isMovingToClickPosition = true;
            }
            
        }

        // Handle movement based on input method
        if (isMovingToClickPosition)
        {
            // Move towards click position
            HandleClickMovement();
        }
        else
        {
            // Traditional keyboard movement
            HandleKeyboardMovement();
        }
    }

    // �·�������ȡ���������ת�������������
    Vector3 GetMouseWorldPosition()
    {
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -Camera.main.transform.position.z;

        // ת��Ϊ��������
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        return worldPos;
    }

    void HandleKeyboardMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // �޸������������ת�ļ������뷽��
        Vector2 inputDirection = new Vector2(inputX, inputY).normalized;
        // �����뷽��ת��Ϊ���������ת�����緽��
        Vector2 movementDirection = GetAdjustedMovementDirection(inputDirection);

        rigidbody.velocity = movementDirection * speed;

        UpdateAnimationState(movementDirection, inputX, inputY);
    }

    // �·��������������ת�����ƶ�����
    Vector2 GetAdjustedMovementDirection(Vector2 inputDir)
    {
        // ��ȡ�����ת�Ƕ�
        float cameraAngle = Camera.main.transform.eulerAngles.z;

        // ������ת����
        float angleInRadians = cameraAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        // Ӧ����ת�����뷽��
        Vector2 rotatedDir = new Vector2(
            inputDir.x * cosAngle - inputDir.y * sinAngle,
            inputDir.x * sinAngle + inputDir.y * cosAngle
        );

        return rotatedDir.normalized;
    }

    void HandleClickMovement()
    {
        // Calculate direction to target
        Vector2 currentPosition = rigidbody.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        // Check if we've arrived at the target
        if (distanceToTarget <= stoppingDistance)
        {
            rigidbody.velocity = Vector2.zero;
            isMovingToClickPosition = false;
            UpdateAnimationState(Vector2.zero, 0, 0);
            return;
        }

        // Move towards target
        rigidbody.velocity = direction * clickMoveSpeed;

        // ���㿼�������ת������ֵ���ڶ���
        Vector2 inputDirection = CalculateInputFromWorldDirection(direction);

        // Update animation based on movement direction
        UpdateAnimationState(direction, inputDirection.x, inputDirection.y);

        // If keyboard input is detected, switch back to keyboard control
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            isMovingToClickPosition = false;
        }
    }

    // �·����������緽�������ʺ϶���������ֵ
    Vector2 CalculateInputFromWorldDirection(Vector2 worldDir)
    {
        // ��ȡ�����ת�Ƕ�
        float cameraAngle = Camera.main.transform.eulerAngles.z;

        // ��������ת����
        float angleInRadians = -cameraAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        // Ӧ������ת����ȡ���������ķ���
        Vector2 inputDir = new Vector2(
            worldDir.x * cosAngle - worldDir.y * sinAngle,
            worldDir.x * sinAngle + worldDir.y * cosAngle
        );

        return inputDir.normalized;
    }

    void UpdateAnimationState(Vector2 movement, float dirX, float dirY)
    {
        if (movement != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("InputX", dirX);
            animator.SetFloat("InputY", dirY);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            // �������ķ��򣬲�����InputX��InputY
        }
    }
}