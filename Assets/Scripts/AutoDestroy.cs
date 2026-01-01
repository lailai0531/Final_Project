using UnityEngine;
public class AutoDestroy : MonoBehaviour
{
    public float delay = 2.0f; // 2秒後自動消失
    void Start()
    {
        Destroy(gameObject, delay);
    }
}