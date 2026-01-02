using UnityEngine;
using TMPro; 

public class FloatingText : MonoBehaviour
{
    [Header("動畫設定")]
    public float moveSpeed = 2f;    
    public float fadeSpeed = 2f;   
    public float destroyTime = 1.5f; 

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
            textColor = color; 
        }
    }

    void Update()
    {
        
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        
        if (textComponent != null)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textComponent.color = textColor;
        }
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}