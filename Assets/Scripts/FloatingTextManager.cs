using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [Header("連結 Prefab")]
    public GameObject floatingTextPrefab; // ⭐ 把剛剛做好的 Prefab 拖進來

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ⭐ 呼叫這個函式就可以產生飄字
    public void ShowText(string content, Vector3 position, Color color)
    {
        if (floatingTextPrefab != null)
        {
            // 在指定位置生成
            GameObject go = Instantiate(floatingTextPrefab, position, Quaternion.identity);

            // 取得腳本並設定文字
            var script = go.GetComponent<FloatingText>();
            if (script != null)
            {
                script.Setup(content, color);
            }
        }
    }
}