using UnityEngine;

public class CinemachineFPSWalker : MonoBehaviour
{
    // 因為不需要移動，moveSpeed, CharacterController 和 mainCameraTransform 都不需要了

    void Start()
    {
        // 遊戲開始時：隱藏滑鼠並鎖定在螢幕中央
        // 這是為了讓 Cinemachine 可以讀取滑鼠移動來轉動鏡頭
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 移除了 WASD 移動邏輯

        // --- 保留解鎖滑鼠的功能 ---
        // 按下 Left Alt 可以切換滑鼠顯示/隱藏 (方便你點擊 UI 或暫停)
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // 解鎖：顯示滑鼠，通常用於點擊選單
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // 鎖定：隱藏滑鼠，回到 FPS 視角控制
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}

