using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ⭐ 1. 必加：引用輸入系統
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndingManager : MonoBehaviour
{
    [Header("生成位置")]
    public Transform spawnPoint;

    [Header("場景設定")]
    public string startSceneName = "StartMenu"; // ⭐ 請填寫你的開始畫面 Scene 名稱

    [Header("UI 設定")]
    public TextMeshProUGUI finalScoreText; // 顯示分數
    public TextMeshProUGUI roleTitleText;  // 顯示獲得稱號
    public GameObject pauseMenuPanel;      // ⭐ 請拖入暫停選單 Panel
    public float fadeDuration = 1.5f;      // 淡入/淡出時間
    public float displayDuration = 2.0f;   // 停留時間

    // --- 階級設定 ---
    [System.Serializable]
    public class Rank
    {
        public string rankName;   // 階級名稱
        public int minScore;      // 最低分數
        public GameObject prefab; // 對應 Prefab
    }

    [Header("階級設定 (請在 Inspector 設定)")]
    public List<Rank> ranks = new List<Rank>();

    // 狀態變數
    private bool isPaused = false;

    void Start()
    {
        // 0. 初始化
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(false);

        // 1. UI 顯示分數 (略) ...
        float finalScore = GameFlow.totalCash;
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;

        // 2. 決定角色 (略) ...
        GameObject prefabToSpawn = null;
        string currentRankName = "";

        // ... (這裡保留你原本的排序邏輯) ...
        if (ranks.Count > 0)
        {
            prefabToSpawn = ranks[ranks.Count - 1].prefab;
            currentRankName = ranks[ranks.Count - 1].rankName;
        }
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

        // --- 3. ⭐ 修改重點：生成角色與攝影機判斷 ---
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject playerObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            // 檢查這個生成的東西，是不是漢堡
            var burgerScript = playerObj.GetComponent<BurgerEndingBehavior>();

            // 取得主攝影機腳本
            var camScript = (Camera.main != null) ? Camera.main.GetComponent<ThirdPersonCamera>() : null;

            if (burgerScript != null)
            {
                // 🔥 A. 如果是漢堡：
                // 絕對不要設定 camScript.target！
                // 甚至要「主動關閉」攝影機腳本，以免它亂動
                if (camScript != null)
                {
                    camScript.target = null;   // 清空目標
                    camScript.enabled = false; // 立即關閉追蹤功能
                }

                // 漢堡自己的 Start() 會負責接手後續的運鏡
            }
            else
            {
                // B. 如果是普通角色 (富翁/乞丐)：
                // 照常啟用第三人稱攝影機
                if (camScript != null)
                {
                    camScript.enabled = true; // 確保它是開的
                    camScript.target = playerObj.transform; // 綁定目標
                }
            }
        }

        // 4. UI 動畫 (略) ...
        if (roleTitleText != null)
        {
            roleTitleText.text = currentRankName;
            roleTitleText.alpha = 0;
            StartCoroutine(FadeTextRoutine());
        }
    }

    // ⭐ 新增 Update 偵測暫停
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // =======================
    // 暫停系統邏輯
    // =======================
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        if (isPaused)
        {
            // 暫停狀態：凍結時間，顯示滑鼠
            Time.timeScale = 0f;
            SetCursorState(true);
        }
        else
        {
            // 遊戲狀態：恢復時間，隱藏滑鼠
            Time.timeScale = 1f;
            SetCursorState(false);
        }
    }

    // 小工具：切換滑鼠顯示/隱藏
    private void SetCursorState(bool show)
    {
        if (show)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // =======================
    // UI 按鈕事件
    // =======================

    // 1. Continue 按鈕
    public void OnContinueClicked()
    {
        TogglePause(); // 直接呼叫切換，就會關閉選單並恢復
    }

    // 2. Restart 按鈕 (回到最一開始的場景)
    public void OnRestartClicked()
    {
        Time.timeScale = 1f; // 恢復時間很重要，不然下一關會卡住

        GameFlow.ResetStatics();
        GameFlow.gameTime = 0f;

        SceneManager.LoadScene(startSceneName);
    }

    // 3. Exit 按鈕
    public void OnExitClicked()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // 原本的回主選單按鈕 (如果有保留這顆按鈕的話可沿用，功能同 Restart)
    public void GoToMainMenu()
    {
        OnRestartClicked();
    }

    // =======================
    // 動畫邏輯 (維持不變)
    // =======================
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
}