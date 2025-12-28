using UnityEngine;
using UnityEngine.InputSystem; // 引用輸入系統

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // 跟隨目標

    [Header("跟隨設定")]
    public float distance = 8.0f;       // 距離目標多遠
    public float height = 5.0f;         // 距離目標多高 (初始高度)
    public float heightDamping = 2.0f;  // 高度平滑
    public float rotationDamping = 3.0f;// 旋轉平滑

    [Header("滑鼠控制")]
    public bool useMouseRotation = true; // 是否啟用滑鼠旋轉
    public float rotationSpeed = 2.0f;   // 旋轉靈敏度

    // 內部變數用來記錄角度
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        // 初始化角度
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. 讀取滑鼠輸入 (旋轉視角)
        if (useMouseRotation)
        {
            // 讀取滑鼠移動量
            float mouseX = Mouse.current.delta.x.ReadValue();
            float mouseY = Mouse.current.delta.y.ReadValue();

            // 如果按住右鍵才旋轉 (選用，若想隨時旋轉就把 if 拿掉)
            currentX += mouseX * rotationSpeed;
            currentY -= mouseY * rotationSpeed;

            // 限制抬頭低頭的角度
            currentY = Mathf.Clamp(currentY, -10, 60);
        }

        // 2. 計算目標位置與旋轉
        // 根據目前的滑鼠角度 (currentX) 來決定相機要轉到哪裡
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 理想位置：目標位置 - (旋轉角度 * 距離) + 高度偏移
        // 這裡做一個簡單的 Orbit (軌道) 計算
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // 3. 更新相機
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationDamping);

        // 4. 讓相機看著目標的頭部 (+1.5f)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}