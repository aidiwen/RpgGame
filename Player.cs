using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("角色速度")]
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
                // 修复：考虑相机旋转的鼠标点击位置计算
                Vector3 mouseWorldPos = GetMouseWorldPosition();
                targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                /* 调试输出
                Debug.Log("相机旋转角度: " + Camera.main.transform.eulerAngles.z +
                         ", 目标位置: " + targetPosition +
                         ", 当前位置: " + transform.position);
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

    // 新方法：获取考虑相机旋转的鼠标世界坐标
    Vector3 GetMouseWorldPosition()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -Camera.main.transform.position.z;

        // 转换为世界坐标
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        return worldPos;
    }

    void HandleKeyboardMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // 修复：考虑相机旋转的键盘输入方向
        Vector2 inputDirection = new Vector2(inputX, inputY).normalized;
        // 将输入方向转换为考虑相机旋转的世界方向
        Vector2 movementDirection = GetAdjustedMovementDirection(inputDirection);

        rigidbody.velocity = movementDirection * speed;

        UpdateAnimationState(movementDirection, inputX, inputY);
    }

    // 新方法：根据相机旋转调整移动方向
    Vector2 GetAdjustedMovementDirection(Vector2 inputDir)
    {
        // 获取相机旋转角度
        float cameraAngle = Camera.main.transform.eulerAngles.z;

        // 创建旋转矩阵
        float angleInRadians = cameraAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        // 应用旋转到输入方向
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

        // 计算考虑相机旋转的输入值用于动画
        Vector2 inputDirection = CalculateInputFromWorldDirection(direction);

        // Update animation based on movement direction
        UpdateAnimationState(direction, inputDirection.x, inputDirection.y);

        // If keyboard input is detected, switch back to keyboard control
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            isMovingToClickPosition = false;
        }
    }

    // 新方法：从世界方向计算回适合动画的输入值
    Vector2 CalculateInputFromWorldDirection(Vector2 worldDir)
    {
        // 获取相机旋转角度
        float cameraAngle = Camera.main.transform.eulerAngles.z;

        // 创建逆旋转矩阵
        float angleInRadians = -cameraAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        // 应用逆旋转来获取相对于相机的方向
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
            // 保留最后的方向，不更新InputX和InputY
        }
    }
}