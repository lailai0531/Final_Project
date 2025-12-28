using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // ⭐ 1. 必加這行，才能切換場景

public class MainController : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 60f; // ⭐ 設定遊戲時間 (例如 60 秒)
    public string nextSceneName = "Level2"; // ⭐ 下一關的場景名稱

    [Header("UI References")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI scoreText; // 這是暫停頁面的分數

    public GameObject gameHUD;

    // ⭐ 新增結算畫面參考
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Targets Prefab")]
    public GameObject targetsPrefab;

    private GameObject currentTargets;

    private bool isPaused = false;
    private bool isGameStarted = false;
    private bool isGameOver = false; // ⭐ 防止重複觸發結束

    private int score = 0;

    public MonoBehaviour[] playerScripts;

    void Start()
    {
        Time.timeScale = 0f;
        isGameStarted = false;

        // 確保結算畫面一開始是關閉的
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        ShowStartMenu();
        if (gameHUD != null) gameHUD.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        // 如果遊戲還沒開始，或是已經結束了，就不跑下面的邏輯
        if (!isGameStarted || isGameOver) return;

        // ⭐ 檢查時間是否到了
        if (GameFlow.gameTime >= gameDuration)
        {
            EndGame(); // 時間到！執行結束邏輯
        }

        // 偵測 ESC 暫停
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // =======================
    // ⭐ 新增：遊戲結束邏輯
    // =======================
    private void EndGame()
    {
        isGameOver = true;
        isGameStarted = false; // 停止 Update 裡的偵測
        Time.timeScale = 0f;   // 暫停時間

        // 1. 處理滑鼠 (這很重要，不然玩家點不到按鈕)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 2. 停用主角控制
        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = false;
        }

        // 3. 關閉 HUD，顯示結算畫面
        if (gameHUD != null) gameHUD.SetActive(false);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // 顯示最終分數
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: ${GameFlow.totalCash:0}";
            }
        }
    }

    // =======================
    // ⭐ 新增：切換場景 (給結算畫面的按鈕用)
    // =======================
    public void OnNextLevelBtnClick()
    {
        // 記得把時間流動改回 1，不然下個場景會動不了
        Time.timeScale = 1f;

        // 載入下一個場景
        SceneManager.LoadScene(nextSceneName);
    }

    // =======================
    // UI Button Events
    // =======================

    public void OnStartBtnClick()
    {
        StartGame();
    }

    public void OnRestartBtnClick()
    {
        RestartGame();
    }

    public void OnContinueBtnClick()
    {
        EnterGameplayState();
    }

    public void OnExitBtnClick()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // =======================
    // Game Flow
    // =======================

    private void StartGame()
    {
        ResetScore();
        ActivateTargets();

        GameFlow.ResetStatics();

        isGameOver = false; // ⭐ 重置結束狀態
        EnterGameplayState();
    }

    private void RestartGame()
    {
        StopAllCoroutines();

        if (currentTargets != null)
            Destroy(currentTargets);

        GameFlow.ResetGameFlow();

        ResetScore();
        ActivateTargets();

        isGameOver = false; // ⭐ 重置結束狀態
        EnterGameplayState();
    }

    private void EnterGameplayState()
    {
        isGameStarted = true;
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        // 3. 鎖定滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 4. 恢復主角控制
        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = true;
        }

        startMenu.SetActive(false);
        pauseMenu.SetActive(false);

        // ⭐ 確保結算畫面是關的，HUD 是開的
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
    }

    private void TogglePause()
    {
        // 如果遊戲已經結束，就不允許切換暫停狀態
        if (isGameOver) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            // 顯示滑鼠
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 停用主角控制
            foreach (var script in playerScripts)
            {
                if (script != null) script.enabled = false;
            }

            if (scoreText != null)
            {
                scoreText.text = $"Score : {GameFlow.totalCash:0}";
            }
        }
        else
        {
            EnterGameplayState();
        }
    }

    private void ShowStartMenu()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false); // ⭐
    }

    // =======================
    // Targets & Score
    // =======================

    private void ActivateTargets()
    {
        if (currentTargets != null)
        {
            Destroy(currentTargets);
        }
        //currentTargets = Instantiate(targetsPrefab);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }
}