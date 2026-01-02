using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("¤¬°Ê³]©w")]
    public float interactRange = 5f;
    public LayerMask interactLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            ShootRay(false); 
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Cursor.lockState == CursorLockMode.None) return;
            ShootRay(true); 
        }
    }

    void ShootRay(bool isRightClick)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            ServePlate plate = hit.transform.GetComponent<ServePlate>();

            if (plate != null)
            {
                if (isRightClick)
                {
                    plate.ClearPlate();
                }
                else
                {
                    plate.Interact();
                }
                return;
            }

            if (isRightClick) return;


            clickplace foodBtn = hit.transform.GetComponent<clickplace>();
            if (foodBtn != null) { foodBtn.Interact(); return; }

            CookMove cookedMeat = hit.transform.GetComponent<CookMove>();
            if (cookedMeat != null) { cookedMeat.Interact(); return; }

            burger_uncooked_con rawMeatBox = hit.transform.GetComponent<burger_uncooked_con>();
            if (rawMeatBox != null) { rawMeatBox.Interact(); return; }
        }
    }
}