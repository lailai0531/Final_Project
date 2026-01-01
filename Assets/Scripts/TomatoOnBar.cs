using UnityEngine;

public class TomatoBar : MonoBehaviour
{
    [Header("設定")]
    public GameObject boardTomatoPrefab;
    public Transform boardPosition;

    [Header("音效")]
    // 修改 1: 改成 AudioSource，跟 clickplace 一樣
    [SerializeField] private AudioSource moveAudio;

    private void OnMouseDown()
    {
        // --- Debug 1: 測試有沒有感應到點擊 ---
        Debug.Log("【測試】滑鼠點到吧台番茄了！物件名稱：" + gameObject.name);

        // 檢查 Prefab 有沒有拉
        if (boardTomatoPrefab == null)
        {
            Debug.LogError("【錯誤】Board Tomato Prefab 是空的！請去 Inspector 拉進去！");
            return;
        }

        // 修改 2: 使用 AudioSource 播放 (跟 clickplace 一樣的寫法)
        if (moveAudio != null)
        {
            // 確保 AudioSource 裡面有放音效檔
            if (moveAudio.clip != null)
            {
                moveAudio.PlayOneShot(moveAudio.clip);
                Debug.Log("【測試】播放音效成功");
            }
            else
            {
                Debug.LogWarning("【警告】AudioSource 元件掛了，但裡面沒有放音效檔 (AudioClip)！");
            }
        }
        else
        {
            Debug.LogWarning("【警告】沒聲音，因為 Move Audio 沒拉 AudioSource！");
        }

        // 2. 生成新番茄
        Vector3 spawnPos = (boardPosition != null) ? boardPosition.position : new Vector3(0, 1.75f, -1.8f);
        Instantiate(boardTomatoPrefab, spawnPos, Quaternion.identity);

        Debug.Log("【測試】生成新番茄成功");
    }
}