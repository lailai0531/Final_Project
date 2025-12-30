using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 60f; // 遊戲限時
    public string nextSceneName = "Level2"; // 下一關場景名稱

    [Header("UI References")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI scoreText; // HUD 或 暫停介面的分數
    public GameObject gameHUD;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Targets Prefab")]
    public GameObject targetsPrefab;
    private GameObject currentTargets;

    private bool isPaused = false;
    private bool isGameStarted = false;
    private bool isGameOver = false;

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
        // 沒開始 或 已結束 就不執行
        if (!isGameStarted || isGameOver) return;

        // 1. 時間流動
        GameFlow.gameTime += Time.deltaTime;

        // 2. 檢查時間是否到了
        if (GameFlow.gameTime >= gameDuration)
        {
            EndGame();
        }

        // 3. 偵測 ESC 暫停
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // =======================
    // 遊戲結束與結算
    // =======================
    private void EndGame()
    {
        isGameOver = true;
        isGameStarted = false;
        Time.timeScale = 0f;

        // 解鎖滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 停用主角
        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = false;
        }

        // 關閉 HUD，顯示結算
        if (gameHUD != null) gameHUD.SetActive(false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (finalScoreText != null)
            {
                finalScoreText.text = $"Gong Der: {GameFlow.totalCash:0}";
            }
        }
    }

    // =======================
    // 場景跳轉 (給結算畫面的 Next Level 按鈕用)
    // =======================
    public void OnNextLevelBtnClick()
    {
        // 恢復時間，避免下一關卡住
        Time.timeScale = 1f;

        // 重置靜態變數
        //GameFlow.ResetStatics();
        //GameFlow.gameTime = 0f;

        SceneManager.LoadScene(nextSceneName);
    }

    // =======================
    // UI 按鈕事件 (Restart 已移除)
    // =======================

    public void OnStartBtnClick()
    {
        StartGame();
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
    // Game Flow (Restart 邏輯已移除)
    // =======================

    private void StartGame()
    {
        ResetScore();
        ActivateTargets();

        GameFlow.ResetStatics();
        isGameOver = false;

        EnterGameplayState();
    }

    private void EnterGameplayState()
    {
        isGameStarted = true;
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        // 鎖定滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 啟用主角
        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = true;
        }

        // 切換 UI
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
    }

    private void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            foreach (var script in playerScripts)
            {
                if (script != null) script.enabled = false;
            }

            if (scoreText != null)
            {
                scoreText.text = $"Gong Der : {GameFlow.totalCash:0}";
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
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void ActivateTargets()
    {
        if (currentTargets != null)
        {
            Destroy(currentTargets);
        }
        // currentTargets = Instantiate(targetsPrefab);
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
            scoreText.text = $"Gong Der : {score}";
        }
    }
}