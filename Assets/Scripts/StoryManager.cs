using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 引用新版輸入系統
using TMPro; // ⭐ 1. 必加：引用 TextMeshPro
using System.Collections;

public class StoryManager : MonoBehaviour
{
    [Header("UI 面板")]
    public GameObject startMenuPanel;   // 開始選單
    public GameObject storyParentPanel; // 故事模式的父物件

    [Header("故事分頁 (請按順序拖入)")]
    public GameObject[] storyPages;

    [Header("打字機設定")]
    public float typingSpeed = 0.05f; // 每個字出現的間隔時間

    [Header("遊戲場景名稱")]
    public string gameSceneName = "Level1";

    private int currentPageIndex = 0;
    private bool isStoryActive = false; // 是否正在看故事
    private bool isTyping = false;      // 是否正在打字中

    // 暫存當前頁面的文字元件與內容
    private TextMeshProUGUI currentTextComponent;
    private string currentFullText = "";
    private Coroutine typingCoroutine;

    void Start()
    {
        // 初始化：顯示選單，隱藏故事
        startMenuPanel.SetActive(true);
        storyParentPanel.SetActive(false);
        isStoryActive = false;
    }

    void Update()
    {
        // 如果沒在看故事，就不執行點擊偵測
        if (!isStoryActive) return;

        // ⭐ 偵測滑鼠左鍵點擊 (任意處)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    // =======================
    // 核心邏輯
    // =======================

    private void HandleClick()
    {
        if (isTyping)
        {
            // 情況 A：還在打字中 -> 瞬間顯示全文字 (略過打字動畫)
            CompleteTypingImmediately();
        }
        else
        {
            // 情況 B：已經打完字 -> 跳下一頁
            GoToNextPage();
        }
    }

    public void OnStartMenuClicked()
    {
        startMenuPanel.SetActive(false);
        storyParentPanel.SetActive(true);

        isStoryActive = true;
        currentPageIndex = 0;

        // 開始顯示第一頁
        ShowPage(currentPageIndex);
    }

    // 顯示指定頁面
    private void ShowPage(int index)
    {
        // 1. 先把所有頁面關閉
        foreach (var page in storyPages)
        {
            page.SetActive(false);
        }

        // 2. 開啟當前頁面
        GameObject activePage = storyPages[index];
        activePage.SetActive(true);

        // 3. ⭐ 尋找該頁面底下的文字元件 (TextMeshProUGUI)
        // 假設你的文字是放在 Page 物件底下，或是 Page 本身有文字
        currentTextComponent = activePage.GetComponentInChildren<TextMeshProUGUI>();

        if (currentTextComponent != null)
        {
            // 把原本已經寫在 Inspector 裡的字存起來
            currentFullText = currentTextComponent.text;

            // 清空文字，準備開始打字
            currentTextComponent.text = "";

            // 啟動打字協程
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText());
        }
        else
        {
            // 如果這頁沒有文字元件，就直接視為打字完成
            isTyping = false;
        }
    }

    // ⭐ 打字機效果的協程
    IEnumerator TypeText()
    {
        isTyping = true;

        foreach (char letter in currentFullText.ToCharArray())
        {
            currentTextComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // 瞬間顯示所有文字
    private void CompleteTypingImmediately()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (currentTextComponent != null)
        {
            currentTextComponent.text = currentFullText;
        }

        isTyping = false;
    }

    private void GoToNextPage()
    {
        // 如果還不是最後一頁，就跳下一頁
        if (currentPageIndex < storyPages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
        else
        {
            // 已經是最後一頁 -> 進入遊戲
            EnterGame();
        }
    }

    // =======================
    // UI 按鈕事件
    // =======================

    // 給 Skip 按鈕呼叫
    public void OnSkipClicked()
    {
        EnterGame();
    }

    private void EnterGame()
    {
        Debug.Log("劇情結束，進入遊戲！");
        SceneManager.LoadScene(gameSceneName);
    }
}