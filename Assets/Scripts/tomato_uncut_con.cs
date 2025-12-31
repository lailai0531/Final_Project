using UnityEngine;

public class tomato_uncut_con : MonoBehaviour
{
    [Header("設定")]
    public Transform cutTomatoPrefab; // 切好的番茄 Prefab

    [Header("音效")]
    public AudioClip chopSound;
    [Range(0, 1)] public float volume = 1.0f;

    private void OnMouseDown()
    {
        // 1. 檢查 Prefab
        if (cutTomatoPrefab == null)
        {
            Debug.LogError("【錯誤】切好番茄的 Prefab 沒拉！");
            return;
        }

        // 2. 播放音效
        if (chopSound != null)
        {
            AudioSource.PlayClipAtPoint(chopSound, transform.position, volume);
        }

        // 3. 【核心邏輯】就在「原本番茄的位置」生成
        // transform.position = 我現在在哪，新的就在哪
        // transform.rotation = 我原本怎麼轉，新的就怎麼轉 (如果你希望切好的番茄轉向一致，這裡改成 cutTomatoPrefab.rotation)
        Instantiate(cutTomatoPrefab, transform.position, transform.rotation);

        Debug.Log("已在座標 " + transform.position + " 生成切好番茄");

        // 4. 舊的消失
        Destroy(gameObject);
    }
}