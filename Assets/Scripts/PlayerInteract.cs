using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 除錯紅線
        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            // 1. 食材按鈕 (clickplace)
            clickplace foodBtn = hit.transform.GetComponent<clickplace>();
            if (foodBtn != null)
            {
                foodBtn.Interact();
                return;
            }

            // 2. 煎檯上的肉 (CookMove)
            CookMove cookedMeat = hit.transform.GetComponent<CookMove>();
            if (cookedMeat != null)
            {
                cookedMeat.Interact();
                return;
            }

            // 3. 出餐盤子 (ServePlate)
            ServePlate plate = hit.transform.GetComponent<ServePlate>();
            if (plate != null)
            {
                plate.Interact();
                return;
            }

            // --- 【新增】 4. 生肉生成箱 (burger_uncooked_con) ---
            burger_uncooked_con rawMeatBox = hit.transform.GetComponent<burger_uncooked_con>();
            if (rawMeatBox != null)
            {
                rawMeatBox.Interact();
                return;
            }
        }
    }
}
