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
        // ⭐ 一定在任何 SetText 前執行
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

    // 只在這裡設定文字 & 啟動生命週期
    public void SetText(string text, Color color)
    {
        if (textComponent == null) return;

        textComponent.text = text;
        textComponent.color = color;

        initialized = true;

        // ⭐ 從這一刻才開始倒數死亡
        Destroy(gameObject, destroyTime);
    }
}
