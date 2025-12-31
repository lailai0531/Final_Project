using UnityEngine;

public class TomatoBar : MonoBehaviour
{
    [Header("設定")]
    public GameObject boardTomatoPrefab;
    public Transform boardPosition;

    [Header("音效")]
    public AudioClip moveSound;
    [Range(0, 1)] public float volume = 1.0f;

    private void OnMouseDown()
    {
        // --- Debug 1: 測試有沒有感應到點擊 ---
        Debug.Log("【測試】滑鼠點到吧台番茄了！物件名稱：" + gameObject.name);

        // 檢查 Prefab 有沒有拉
        if (boardTomatoPrefab == null)
        {
            Debug.LogError("【錯誤】Board Tomato Prefab 是空的！請去 Inspector 拉進去！");
            return; // 停在這裡不要往下跑
        }

        // 1. 播放移動音效
        if (moveSound != null)
        {
            AudioSource.PlayClipAtPoint(moveSound, transform.position, volume);
            Debug.Log("【測試】播放音效成功");
        }
        else
        {
            Debug.LogWarning("【警告】沒聲音，因為 Move Sound 沒拉音效檔");
        }

        // 2. 生成新番茄
        Vector3 spawnPos = (boardPosition != null) ? boardPosition.position : new Vector3(0, 1.75f, -1.8f);
        Instantiate(boardTomatoPrefab, spawnPos, Quaternion.identity);

        Debug.Log("【測試】生成新番茄成功，準備自我銷毀");
    }
}