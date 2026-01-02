using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public static bool isGameRunning = false;

    [Header("Game Settings")]
    public float gameDuration = 60f; 
    public string nextSceneName = "EndScene"; 

    [Header("Player Control")]
    public GameObject playerObject; 
    public MonoBehaviour[] playerScripts; 

    [Header("UI References")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI scoreText; 
    public GameObject gameHUD;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Targets")]
    public GameObject targetsPrefab;
    private GameObject currentTargets;

    private bool isPaused = false;
    public bool isGameStarted = false;
    private bool isGameOver = false;
    private int score = 0;

    void Start()
    {
        Time.timeScale = 0f;
        isGameStarted = false;
        isGameRunning = false;

        if (playerObject != null) playerObject.SetActive(false);

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        ShowStartMenu(); 
        if (gameHUD != null) gameHUD.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return;

        GameFlow.gameTime += Time.deltaTime;

        if (GameFlow.gameTime >= gameDuration)
        {
            EndGame();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

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
        isGameRunning = true;
        EnterGameplayState();
    }

    private void EnterGameplayState()
    {
        isGameStarted = true;
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerObject != null) playerObject.SetActive(true);

        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = true;
        }

        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
    }

    private void EndGame()
    {
        isGameOver = true;
        isGameStarted = false;
        isGameRunning= false;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (var script in playerScripts)
        {
            if (script != null) script.enabled = false;
        }

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
            if (scoreText != null) scoreText.text = $"Gong Der: {GameFlow.totalCash:0}";
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
        if (scoreText != null) scoreText.text = $"Gong Der: {score}";
    }
}