using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("連結設定")]
    public MainController mainController;

    [Header("UI 元件")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    [Header("倒數警示設定")]
    public float alertTime = 10.0f;         // 剩下幾秒開始警告
    public Color normalColor = Color.white;
    public Color alertColor = Color.red;
    public float flashSpeed = 5.0f;

    [Header("音效設定")]
    public AudioSource audioSource; // ⭐ 請掛上並拖入 AudioSource 元件
    public AudioClip beepSound;     // ⭐ 請拖入逼逼聲的音效檔
    private int lastBeepSecond = -1; // 用來記錄上次逼逼是在第幾秒

    void Update()
    {
        // 1. 更新金錢
        if (moneyText != null)
        {
            moneyText.text = $"Gong Der: {GameFlow.totalCash:0}";
        }

        // 2. 更新時間 & 閃爍 & 音效
        if (timeText != null && mainController != null)
        {
            // 計算剩餘時間
            float totalTime = mainController.gameDuration;
            float currentTime = GameFlow.gameTime;
            float remainingTime = totalTime - currentTime;

            // 防呆
            if (remainingTime < 0) remainingTime = 0;

            // 顯示文字
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";

            // === 警示邏輯 (閃爍 + 聲音) ===
            if (remainingTime <= alertTime && remainingTime > 0)
            {
                // A. 視覺閃爍
                float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
                timeText.color = Color.Lerp(normalColor, alertColor, t);

                // B. 聽覺逼逼聲 ⭐
                // 我們取剩餘時間的 "無條件進位整數" (例如 9.9秒 -> 10, 9.1秒 -> 10)
                int currentCeilSecond = Mathf.CeilToInt(remainingTime);

                // 如果現在的秒數跟上次紀錄的不一樣，代表過了一秒，播放聲音！
                if (currentCeilSecond != lastBeepSecond)
                {
                    if (audioSource != null && beepSound != null)
                    {
                        audioSource.PlayOneShot(beepSound);
                    }
                    lastBeepSecond = currentCeilSecond; // 更新紀錄
                }
            }
            else
            {
                // 還沒到警示時間，或是時間到了
                timeText.color = normalColor;
                lastBeepSecond = -1; // 重置聲音狀態
            }
        }
    }
}