using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndingManager : MonoBehaviour
{
    [Header("生成位置")]
    public Transform spawnPoint;

    [Header("UI 設定")]
    public TextMeshProUGUI finalScoreText; // 顯示分數
    public TextMeshProUGUI roleTitleText;  // 顯示獲得稱號
    public float fadeDuration = 1.5f;      // 淡入/淡出時間
    public float displayDuration = 2.0f;   // 停留時間

    // --- ❌原本錯誤的地方 ---
    // [Header("階級設定")]  <-- 這行不能放在 class 上面，要拿掉或往下移

    // --- ✅修正後的寫法 ---
    [System.Serializable]
    public class Rank
    {
        public string rankName;   // 階級名稱
        public int minScore;      // 最低分數
        public GameObject prefab; // 對應 Prefab
    }

    // ⭐ Header 要放在這裡 (List 變數的頭上)
    [Header("階級設定 (請在 Inspector 設定)")]
    public List<Rank> ranks = new List<Rank>();

    void Start()
    {
        // --- 1. 隱藏並鎖定鼠標 ---
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // --- 2. 顯示分數 ---
        float finalScore = GameFlow.totalCash;
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }

        // --- 3. 決定角色 ---
        GameObject prefabToSpawn = null;
        string currentRankName = "";

        // 預設最低階 (避免清單為空時報錯)
        if (ranks.Count > 0)
        {
            prefabToSpawn = ranks[ranks.Count - 1].prefab;
            currentRankName = ranks[ranks.Count - 1].rankName;
        }

        // 排序並比對 (由高分到低分)
        var sortedRanks = ranks.OrderByDescending(x => x.minScore).ToList();
        foreach (var rank in sortedRanks)
        {
            if (finalScore >= rank.minScore)
            {
                prefabToSpawn = rank.prefab;
                currentRankName = rank.rankName;
                break;
            }
        }

        // --- 4. 生成角色與攝影機 ---
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject playerObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            if (Camera.main != null)
            {
                var camScript = Camera.main.GetComponent<ThirdPersonCamera>();
                if (camScript != null) camScript.target = playerObj.transform;
            }
        }

        // --- 5. 執行文字淡入淡出動畫 ---
        if (roleTitleText != null)
        {
            roleTitleText.text = currentRankName;
            roleTitleText.alpha = 0;
            StartCoroutine(FadeTextRoutine());
        }
    }

    // 淡入淡出邏輯
    IEnumerator FadeTextRoutine()
    {
        // 淡入
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            roleTitleText.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        roleTitleText.alpha = 1f;

        // 停留
        yield return new WaitForSeconds(displayDuration);

        // 淡出
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            roleTitleText.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        roleTitleText.alpha = 0f;
    }

    // 回主選單按鈕
    public void GoToMainMenu()
    {
        // 解除鼠標鎖定，不然點不到按鈕
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameFlow.ResetStatics();
        GameFlow.gameTime = 0f;
        SceneManager.LoadScene("StartMenu");
    }
}