using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
// 這裡不需要 System.Collections 了，因為不用 Coroutine

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
    public float typingSpeed = 0.05f; // 打字速度

    private int currentPageIndex = 0;
    private bool isStoryActive = false;
    private bool isTyping = false;

    // --- 計時器變數 ---
    private TextMeshProUGUI currentTextComponent;
    private string currentFullText = "";
    private float timer = 0f;      // 累積時間
    private int charIndex = 0;     // 目前打到第幾個字

    void Update()
    {
        // 沒看故事就不執行
        if (!isStoryActive) return;

        // 1. 處理點擊 (加速或下一頁)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }

        // 2. ⭐ 核心打字邏輯 (寫在 Update 裡最穩)
        if (isTyping && currentTextComponent != null)
        {
            // 使用 unscaledDeltaTime (不受 Time.timeScale = 0 影響)
            timer += Time.unscaledDeltaTime;

            // 當累積時間超過打字速度，就多顯示一個字
            while (timer >= typingSpeed && charIndex < currentFullText.Length)
            {
                timer -= typingSpeed; // 扣掉時間
                charIndex++; // 推進索引

                // 更新文字顯示 (取原本字串的前 charIndex 個字)
                currentTextComponent.text = currentFullText.Substring(0, charIndex);
            }

            // 檢查是否打完了
            if (charIndex >= currentFullText.Length)
            {
                isTyping = false;
            }
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
            // 還在打字 -> 瞬間顯示全部
            CompleteTypingImmediately();
        }
        else
        {
            // 打完了 -> 下一頁
            GoToNextPage();
        }
    }

    private void ShowPage(int index)
    {
        foreach (var page in storyPages) page.SetActive(false);

        GameObject activePage = storyPages[index];
        activePage.SetActive(true);

        // 抓取文字元件
        currentTextComponent = activePage.GetComponentInChildren<TextMeshProUGUI>();

        if (currentTextComponent != null)
        {
            currentFullText = currentTextComponent.text; // 存下全文
            currentTextComponent.text = ""; // 清空畫面

            // ⭐ 重置計時器變數
            timer = 0f;
            charIndex = 0;
            isTyping = true;
        }
        else
        {
            isTyping = false;
        }
    }

    // 移除 Coroutine，改用直接設定字串
    private void CompleteTypingImmediately()
    {
        if (currentTextComponent != null)
        {
            currentTextComponent.text = currentFullText;
            charIndex = currentFullText.Length; // 更新索引到底
        }
        isTyping = false;
    }

    private void GoToNextPage()
    {
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
        Debug.Log("劇情結束，通知 MainController 開始遊戲！");
        storyParentPanel.SetActive(false);
        isStoryActive = false;

        if (mainController != null)
        {
            mainController.OnStartBtnClick();
        }
    }
}