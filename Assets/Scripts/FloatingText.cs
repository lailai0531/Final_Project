using UnityEngine;
using TMPro; // 引用 TMP

public class FloatingText : MonoBehaviour
{
    [Header("動畫設定")]
    public float moveSpeed = 2f;    // 往上飄的速度
    public float fadeSpeed = 2f;    // 淡出的速度
    public float destroyTime = 1.5f; // 幾秒後銷毀

    private TextMeshPro textComponent;
    private Color textColor;

    void Awake()
    {
        textComponent = GetComponent<TextMeshPro>();
        if (textComponent != null)
        {
            textColor = textComponent.color;
        }
    }

    public void Setup(string text, Color color)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
            textComponent.color = color;
            textColor = color; // 更新基礎顏色
        }
    }

    void Update()
    {
        // 1. 往上飄
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 2. 淡出 (修改 Alpha 值)
        if (textComponent != null)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textComponent.color = textColor;
        }
    }

    void LateUpdate()
    {
        // 3. (選用) 讓文字永遠面向攝影機，才不會從側面看變成一條線
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }

    void Start()
    {
        // 時間到自動銷毀
        Destroy(gameObject, destroyTime);
    }
}