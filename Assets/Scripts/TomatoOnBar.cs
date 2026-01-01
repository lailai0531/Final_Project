using UnityEngine;

public class TomatoBar : MonoBehaviour
{
    [Header("設定")]
    public GameObject boardTomatoPrefab;
    public Transform boardPosition;

    [Header("音效替身")]
    // 這裡改用 GameObject，因為我们要生成那個藍色方塊(Prefab)
    [SerializeField] private AudioSource placeAudio;


    private void OnMouseDown()
    {
        if (!MainController.isGameRunning) return;
        // 1. 播放音效 (生成替身)
        if (placeAudio != null)
        {
            placeAudio.Play();
        }
        else
        {
            Debug.LogWarning("【警告】沒聲音，請把做好的音效 Prefab 拉進 Sound Prefab 欄位！");
        }

        // 2. 生成新番茄到砧板
        if (boardTomatoPrefab != null)
        {
            Vector3 spawnPos = (boardPosition != null) ? boardPosition.position : new Vector3(0, 1.75f, -1.8f);
            Instantiate(boardTomatoPrefab, spawnPos, Quaternion.identity);
            Debug.Log("【測試】生成新番茄成功");
        }

        // 3. (選用) 這裡如果你要 Destroy 吧台番茄也可以，因為聲音已經交給替身了
        // Destroy(gameObject); 
    }
}