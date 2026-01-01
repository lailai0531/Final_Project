using UnityEngine;

public class tomato_uncut_con : MonoBehaviour
{
    [Header("設定")]
    public Transform cutTomatoPrefab; // 切好的番茄 Prefab

    [Header("音效")]
    // 修改 1: 改成 AudioSource，讓操作介面統一
    [SerializeField] private AudioSource chopAudio;

    // 這個音量控制保留給 PlayClipAtPoint 使用
    [Range(0, 1)] public float volume = 1.0f;

    private void OnMouseDown()
    {
        // 1. 檢查 Prefab
        if (cutTomatoPrefab == null)
        {
            Debug.LogError("【錯誤】切好番茄的 Prefab 沒拉！");
            return;
        }

        // 2. 播放音效 (修改過的邏輯)
        if (chopAudio != null)
        {
            // 確保 AudioSource 裡面有放音效檔
            if (chopAudio.clip != null)
            {
                // 【重要】因為下面馬上要 Destroy，所以不能用 chopAudio.PlayOneShot()
                // 必須改用 PlayClipAtPoint (在原地生成一個暫時的聲音)
                // 我們讀取 chopAudio 裡的 clip 來播放
                AudioSource.PlayClipAtPoint(chopAudio.clip, transform.position, volume);
            }
            else
            {
                Debug.LogWarning("【警告】AudioSource 裡沒有放音效檔 (AudioClip)！");
            }
        }
        else
        {
            Debug.LogWarning("【警告】沒聲音，因為 Chop Audio 欄位沒拉 AudioSource！");
        }

        // 3. 【核心邏輯】在原地生成切好的番茄
        Instantiate(cutTomatoPrefab, transform.position, transform.rotation);

        Debug.Log("已在座標 " + transform.position + " 生成切好番茄");

        // 4. 舊的消失
        Destroy(gameObject);
    }
}