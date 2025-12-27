using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("互動設定")]
    public float interactRange = 5f;
    public LayerMask interactLayer;

    void Update()
    {
        // --- 原本的左鍵 (互動/拿取) ---
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            ShootRay(false); // false 代表不是右鍵
        }

        // --- 【新增】右鍵 (刪除/倒掉) ---
        if (Input.GetMouseButtonDown(1))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            ShootRay(true); // true 代表是右鍵
        }
    }

    // 修改 ShootRay，多一個參數 isRightClick 來判斷是用哪顆按鍵
    void ShootRay(bool isRightClick)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            // 取得盤子腳本
            ServePlate plate = hit.transform.GetComponent<ServePlate>();

            // 如果射線打到的是盤子
            if (plate != null)
            {
                if (isRightClick)
                {
                    // ★ 如果是按右鍵 -> 倒掉食物
                    plate.ClearPlate();
                }
                else
                {
                    // ★ 如果是按左鍵 -> 原本的出餐/互動
                    plate.Interact();
                }
                return; // 處理完盤子就結束，不往下執行
            }

            // --- 如果是右鍵，通常只處理盤子，下面這些可以跳過 ---
            if (isRightClick) return;

            // ... (下面原本的 clickplace, CookMove, rawMeatBox 邏輯保持不變) ...

            clickplace foodBtn = hit.transform.GetComponent<clickplace>();
            if (foodBtn != null) { foodBtn.Interact(); return; }

            CookMove cookedMeat = hit.transform.GetComponent<CookMove>();
            if (cookedMeat != null) { cookedMeat.Interact(); return; }

            burger_uncooked_con rawMeatBox = hit.transform.GetComponent<burger_uncooked_con>();
            if (rawMeatBox != null) { rawMeatBox.Interact(); return; }
        }
    }
}