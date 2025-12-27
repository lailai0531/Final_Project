using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MainController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI scoreText;

    public GameObject gameHUD;

    [Header("Targets Prefab")]
    public GameObject targetsPrefab;

    private GameObject currentTargets;

    private bool isPaused = false;
    private bool isGameStarted = false;

    private int score = 0;

    void Start()
    {
        Time.timeScale = 0f;
        isGameStarted = false;


        ShowStartMenu();
        if (gameHUD != null) gameHUD.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameStarted && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
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
        // 直接呼叫「進入遊戲狀態」，它會負責恢復時間並關閉選單
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
        EnterGameplayState();
    }

    private void RestartGame()
    {
        StopAllCoroutines();

        if (currentTargets != null)
            Destroy(currentTargets);

        GameFlow.ResetGameFlow();   // ⭐ 關鍵

        ResetScore();
        ActivateTargets();
        EnterGameplayState();
    }

    private void EnterGameplayState()
    {
        isGameStarted = true;
        isPaused = false;
        Time.timeScale = 1f;

        startMenu.SetActive(false);
        pauseMenu.SetActive(false);

    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

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

        currentTargets = Instantiate(targetsPrefab);
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
