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
    public float alertTime = 10.0f;         
    public Color normalColor = Color.white;
    public Color alertColor = Color.red;
    public float flashSpeed = 5.0f;

    [Header("音效設定")]
    public AudioSource audioSource; 
    public AudioClip beepSound;     
    private int lastBeepSecond = -1; 

    void Update()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Gong Der: {GameFlow.totalCash:0}";
        }

        if (timeText != null && mainController != null)
        {
            float totalTime = mainController.gameDuration;
            float currentTime = GameFlow.gameTime;
            float remainingTime = totalTime - currentTime;

            if (remainingTime < 0) remainingTime = 0;

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";

            if (remainingTime <= alertTime && remainingTime > 0)
            {
                float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
                timeText.color = Color.Lerp(normalColor, alertColor, t);

                int currentCeilSecond = Mathf.CeilToInt(remainingTime);

                if (currentCeilSecond != lastBeepSecond)
                {
                    if (audioSource != null && beepSound != null)
                    {
                        audioSource.PlayOneShot(beepSound);
                    }
                    lastBeepSecond = currentCeilSecond; 
                }
            }
            else
            {
                timeText.color = normalColor;
                lastBeepSecond = -1;
            }
        }
    }
}