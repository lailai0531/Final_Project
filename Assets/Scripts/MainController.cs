using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 60f; // 遊戲限時
    public string nextSceneName = "Level2"; // 下一關場景名稱

    [Header("Player Control (重要)")]
    public GameObject playerObject; // ⭐ 請把主角的 GameObject 拖進來 (用來控制顯示/隱藏)
    public MonoBehaviour[] playerScripts; // 主角的控制腳本 (用來控制能不能動)

    [Header("UI References")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI scoreText; // HUD 分數
    public GameObject gameHUD;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Targets")]
    public GameObject targetsPrefab;
    private GameObject currentTargets;

    // 狀態變數
    private bool isPaused = false;
    private bool isGameStarted = false;
    private bool isGameOver = false;
    private int score = 0;

    void Start()
    {
        Time.timeScale = 0f;
        isGameStarted = false;

        // 1. 一開始先把主角隱藏 (因為要先看劇情)
        if (playerObject != null) playerObject.SetActive(false);

        // 2. 初始化 UI
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        ShowStartMenu(); // 顯示開始選單
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
    // 遊戲流程 (給 StoryManager 呼叫)
    // =======================

    // 這個函式會被 StoryManager 呼叫，代表劇情結束，遊戲正式開始
    public void OnStartBtnClick()
    {
        StartGame();
    }

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

        // ⭐ 遊戲開始：把主角顯示出來
        if (playerObject != null) playerObject.SetActive(true);

        // 啟用主角控制
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

    // =======================
    // 遊戲結束
    // =======================
    private void EndGame()
    {
        isGameOver = true;
        isGameStarted = false;
        Time.timeScale = 0f;

        // 解鎖滑鼠
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 停用主角控制 (但不需要隱藏主角，屍體要在場上)
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
                finalScoreText.text = $"Score: {GameFlow.totalCash:0}";
            }
        }
    }

    // =======================
    // 其他功能
    // =======================
    public void OnNextLevelBtnClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnContinueBtnClick() { EnterGameplayState(); }

    public void OnExitBtnClick() { Application.Quit(); }

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
            foreach (var script in playerScripts) if (script != null) script.enabled = false;
            if (scoreText != null) scoreText.text = $"Score : {GameFlow.totalCash:0}";
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
        if (currentTargets != null) Destroy(currentTargets);
        // currentTargets = Instantiate(targetsPrefab);
    }

    private void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = $"Score : {score}";
    }
}