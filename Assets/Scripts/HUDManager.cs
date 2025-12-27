using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // 【修改】拆成兩個變數，這樣 Inspector 就會出現兩個欄位
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    void Update()
    {
        // 1. 更新金錢文字
        if (moneyText != null)
        {
            moneyText.text = $"Cash: ${GameFlow.totalCash:0}";
        }

        // 2. 更新時間文字
        if (timeText != null)
        {
            // 計算分與秒
            int minutes = Mathf.FloorToInt(GameFlow.gameTime / 60);
            int seconds = Mathf.FloorToInt(GameFlow.gameTime % 60);

            // 更新文字顯示
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}