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
    public AudioSource audioSource;       
    // public AudioClip typeSound;        
    private int currentPageIndex = 0;
    private bool isStoryActive = false;
    private bool isTyping = false;

    private TextMeshProUGUI currentTextComponent;
    private string currentFullText = "";
    private float timer = 0f;
    private int charIndex = 0;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true; 
            audioSource.Stop();      
        }
    }

    void Update()
    {
        if (!isStoryActive) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }

        if (isTyping && currentTextComponent != null)
        {
            timer += Time.unscaledDeltaTime;

            while (timer >= typingSpeed && charIndex < currentFullText.Length)
            {
                timer -= typingSpeed;
                charIndex++;
                currentTextComponent.text = currentFullText.Substring(0, charIndex);

            }

            if (charIndex >= currentFullText.Length)
            {
                isTyping = false;

                StopTypingSound();
            }
        }
    }

    void StartTypingSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

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

        StopTypingSound();
    }

    private void GoToNextPage()
    {
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
        StopTypingSound(); 

        Debug.Log("劇情結束，通知 MainController 開始遊戲");
        storyParentPanel.SetActive(false);
        isStoryActive = false;

        if (mainController != null)
        {
            mainController.OnStartBtnClick();
        }
    }
}