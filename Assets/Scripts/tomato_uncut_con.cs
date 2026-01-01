using UnityEngine;

public class tomato_uncut_con : MonoBehaviour
{
    [Header("設定")]
    public Transform cutTomatoPrefab; // 切好的番茄 Prefab

    [Header("特效")]
    public GameObject particlePrefab; // ⭐ 新增：請把你的煙霧/碎屑粒子 Prefab 拖進來

    [Header("音效")]
    [SerializeField] private AudioSource chopSFX;

    private void OnMouseDown()
    {
        // (選用) 建議加上遊戲狀態檢查，避免暫停時還能切
        if (!MainController.isGameRunning) return;

        Debug.Log("切菜動作執行");

        if (cutTomatoPrefab == null) return;

        // 1. 生成切好的番茄 (繼承原本的位置和旋轉)
        Instantiate(cutTomatoPrefab, transform.position, transform.rotation);

        // 2. ⭐ 生成粒子特效
        if (particlePrefab != null)
        {
            // 在番茄的位置生成特效，Quaternion.identity 代表不旋轉(世界座標方向)
            GameObject vfx = Instantiate(particlePrefab, transform.position, Quaternion.identity);

            // 2秒後自動銷毀特效物件，避免場景垃圾堆積
            Destroy(vfx, 2.0f);
        }

        // 3. 播放音效
        // 因為這個物件馬上要 Destroy，所以必須用 PlayClipAtPoint，不然聲音會斷掉
        if (chopSFX != null && chopSFX.clip != null)
        {
            AudioSource.PlayClipAtPoint(chopSFX.clip, Camera.main.transform.position, 1.0f);
        }

        // 4. 銷毀原本還沒切的番茄
        Destroy(gameObject);
    }
}