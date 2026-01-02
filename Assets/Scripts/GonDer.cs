using TMPro;
using UnityEngine;

public class FloatingTextAnim : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1f;

    private TextMeshPro textComponent;
    private bool initialized = false;

    void Awake()
    {
        textComponent = GetComponentInChildren<TextMeshPro>();
        if (textComponent == null)
        {
            Debug.LogError("找不到 TextMeshPro！");
        }
    }

    void Update()
    {
        if (!initialized) return;

        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    public void SetText(string text, Color color)
    {
        if (textComponent == null) return;

        textComponent.text = text;
        textComponent.color = color;

        initialized = true;

        Destroy(gameObject, destroyTime);
    }
}
