using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndingManager : MonoBehaviour
{
    [Header("生成位置")]
    public Transform spawnPoint;

    [Header("場景設定")]
    public string startSceneName = "StartMenu"; 

    [Header("UI 設定")]
    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI roleTitleText;  
    public GameObject pauseMenuPanel;      
    public float fadeDuration = 1.5f;      
    public float displayDuration = 2.0f;   

    [System.Serializable]
    public class Rank
    {
        public string rankName;   
        public int minScore;      
        public GameObject prefab; 
    }

    [Header("階級設定")]
    public List<Rank> ranks = new List<Rank>();

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        SetCursorState(false);

        float finalScore = GameFlow.totalCash;
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;

        GameObject prefabToSpawn = null;
        string currentRankName = "";

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

        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject playerObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            var burgerScript = playerObj.GetComponent<BurgerEndingBehavior>();

            var camScript = (Camera.main != null) ? Camera.main.GetComponent<ThirdPersonCamera>() : null;

            if (burgerScript != null)
            {
                
                if (camScript != null)
                {
                    camScript.target = null;   
                    camScript.enabled = false; 
                }

            }
            else
            {
                if (camScript != null)
                {
                    camScript.enabled = true; 
                    camScript.target = playerObj.transform; 
                }
            }
        }

        if (roleTitleText != null)
        {
            roleTitleText.text = currentRankName;
            roleTitleText.alpha = 0;
            StartCoroutine(FadeTextRoutine());
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            SetCursorState(true);
        }
        else
        {
            Time.timeScale = 1f;
            SetCursorState(false);
        }
    }

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


    public void OnContinueClicked()
    {
        TogglePause();
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f; 

        GameFlow.ResetStatics();
        GameFlow.gameTime = 0f;

        SceneManager.LoadScene(startSceneName);
    }

    public void OnExitClicked()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        OnRestartClicked();
    }

    IEnumerator FadeTextRoutine()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            roleTitleText.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        roleTitleText.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

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