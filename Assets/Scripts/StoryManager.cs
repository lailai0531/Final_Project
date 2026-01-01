using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour
{
    [Header("連結 MainController")]
    public MainController mainController;

    [Header("UI 面板")]
    public GameObject startMenuPanel;
    public GameObject storyParentPanel;

    [Header("故事分頁")]
    public GameObject[] storyPages;

    [Header("打字機設定")]
    public float typingSpeed = 0.05f;

    [Header("音效設定")]
    public AudioSource audioSource;       // 請確保這個 AudioSource 在 Inspector 有掛上你的長音效
    // public AudioClip typeSound;        // ⭐ 這裡不需要了，直接用 AudioSource 上的 Clip

    private int currentPageIndex = 0;
    private bool isStoryActive = false;
    private bool isTyping = false;

    // --- 計時器變數 ---
    private TextMeshProUGUI currentTextComponent;
    private string currentFullText = "";
    private float timer = 0f;
    private int charIndex = 0;

    void Start()
    {
        // ⭐ 【重要】確保音效設定為循環播放
        if (audioSource != null)
        {
            audioSource.loop = true; // 設定為循環，這樣它會一直打字直到我們叫它停
            audioSource.Stop();      // 一開始先不要播
        }
    }

    void Update()
    {
        if (!isStoryActive) return;

        // 1. 處理點擊
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }

        // 2. 打字邏輯
        if (isTyping && currentTextComponent != null)
        {
            timer += Time.unscaledDeltaTime;

            while (timer >= typingSpeed && charIndex < currentFullText.Length)
            {
                timer -= typingSpeed;
                charIndex++;
                currentTextComponent.text = currentFullText.Substring(0, charIndex);

                // ⭐ 這裡把原本的 PlayTypingSound() 拿掉了，因為我們改成持續播放
            }

            // 檢查是否打完了
            if (charIndex >= currentFullText.Length)
            {
                isTyping = false;

                // ⭐ 【新增】打字結束，停止音效
                StopTypingSound();
            }
        }
    }

    // ⭐ 【新增】控制音效的函式：開始
    void StartTypingSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // ⭐ 【新增】控制音效的函式：停止
    void StopTypingSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void OnStartMenuClicked()
    {
        startMenuPanel.SetActive(false);
        storyParentPanel.SetActive(true);

        isStoryActive = true;
        currentPageIndex = 0;

        ShowPage(currentPageIndex);
    }

    private void HandleClick()
    {
        if (isTyping)
        {
            CompleteTypingImmediately();
        }
        else
        {
            GoToNextPage();
        }
    }

    private void ShowPage(int index)
    {
        foreach (var page in storyPages) page.SetActive(false);

        GameObject activePage = storyPages[index];
        activePage.SetActive(true);

        currentTextComponent = activePage.GetComponentInChildren<TextMeshProUGUI>();

        if (currentTextComponent != null)
        {
            currentFullText = currentTextComponent.text;
            currentTextComponent.text = "";

            timer = 0f;
            charIndex = 0;
            isTyping = true;

            // ⭐ 【新增】新的一頁開始打字，播放音效
            StartTypingSound();
        }
        else
        {
            isTyping = false;
        }
    }

    private void CompleteTypingImmediately()
    {
        if (currentTextComponent != null)
        {
            currentTextComponent.text = currentFullText;
            charIndex = currentFullText.Length;
        }
        isTyping = false;

        // ⭐ 【新增】玩家強制跳過，立刻停止音效
        StopTypingSound();
    }

    private void GoToNextPage()
    {
        // 確保換頁時音效一定有關掉 (雙重保險)
        StopTypingSound();

        if (currentPageIndex < storyPages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
        else
        {
            EnterGame();
        }
    }

    public void OnSkipClicked()
    {
        EnterGame();
    }

    private void EnterGame()
    {
        StopTypingSound(); // 離開劇情模式也要關聲音

        Debug.Log("劇情結束，通知 MainController 開始遊戲！");
        storyParentPanel.SetActive(false);
        isStoryActive = false;

        if (mainController != null)
        {
            mainController.OnStartBtnClick();
        }
    }
}