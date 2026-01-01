using UnityEngine;
using System.Collections;

public class BurgerEndingBehavior : MonoBehaviour
{
    [Header("模型設定")]
    public GameObject burgerOnPlate; // 盤子上的漢堡
    public GameObject emptyPlate;    // 空盤子

    [Header("掉落設定 (新功能)")]
    public float dropHeight = 5.0f;     // 從多高的地方掉下來
    public float dropDuration = 0.5f;   // 掉落過程要多久 (越小越快)
    public AudioClip landSound;         // 掉到盤子上的聲音 (啪!)

    [Header("鏡頭運鏡設定")]
    public Transform cameraEndAnchor;   // ⭐ 請務必設定這個攝影機終點錨點
    public float moveDuration = 1.0f;   // 鏡頭移動時間

    [Header("吃掉設定")]
    public AudioSource audioSource;
    public AudioClip biteSound;         // 咬下去的聲音

    void Start()
    {
        // 初始化模型狀態
        if (burgerOnPlate != null) burgerOnPlate.SetActive(true);
        if (emptyPlate != null) emptyPlate.SetActive(false);

        // 啟動主流程
        StartCoroutine(MainSequence());
    }

    IEnumerator MainSequence()
    {
        // --- 階段 1: 準備掉落 ---

        // 1. 記錄盤子的位置 (這是終點)
        Vector3 landPosition = transform.position;

        // 2. 把漢堡瞬間移到天空
        Vector3 skyPosition = landPosition + Vector3.up * dropHeight;
        transform.position = skyPosition;

        // --- 階段 2: 掉下來 ---

        float timer = 0f;
        while (timer < dropDuration)
        {
            timer += Time.deltaTime;
            // 使用 EaseIn (越掉越快) 的曲線
            float t = timer / dropDuration;
            t = t * t; // 二次方加速曲線

            transform.position = Vector3.Lerp(skyPosition, landPosition, t);
            yield return null;
        }

        // 確保最後位置完全歸零
        transform.position = landPosition;

        // 播放落地聲 (啪!)
        if (audioSource != null && landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }

        // 稍微震動一下攝影機或停頓一下，增加打擊感
        yield return new WaitForSeconds(0.3f);


        // --- 階段 3: 攝影機運鏡 (跟之前一樣) ---

        Camera mainCam = Camera.main;
        if (mainCam != null && cameraEndAnchor != null)
        {
            // 關閉原本攝影機控制
            var tpCamera = mainCam.GetComponent<ThirdPersonCamera>();
            if (tpCamera != null) tpCamera.enabled = false;

            Vector3 startCamPos = mainCam.transform.position;
            Quaternion startCamRot = mainCam.transform.rotation;

            timer = 0f;
            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                t = Mathf.SmoothStep(0, 1, t); // 平滑移動

                // 飛向錨點
                mainCam.transform.position = Vector3.Lerp(startCamPos, cameraEndAnchor.position, t);
                mainCam.transform.rotation = Quaternion.Slerp(startCamRot, cameraEndAnchor.rotation, t);

                yield return null;
            }
        }

        // --- 階段 4: 咬下去 ---

        yield return new WaitForSeconds(0.7f); // 鏡頭到了之後稍微停一下再咬

        if (audioSource != null && biteSound != null)
        {
            audioSource.PlayOneShot(biteSound);
        }

        if (burgerOnPlate != null) burgerOnPlate.SetActive(false);
        if (emptyPlate != null) emptyPlate.SetActive(true);

        Debug.Log("演出結束：掉落 -> 運鏡 -> 吃掉");
    }
}