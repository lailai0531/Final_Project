using UnityEngine;
using TMPro; // 如果你需要顯示分數
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("角色模型設定 (按爛到好排序)")]
    // 0: 乞丐裝 (低分), 1: 普通裝 (中分), 2: 國王裝 (高分)
    public GameObject[] characterPrefabs;

    [Header("生成位置")]
    public Transform spawnPoint; // 角色要生成在哪裡

    [Header("分數門檻")]
    public int scoreForNormal = 100; // 超過幾分變普通人
    public int scoreForRich = 500;   // 超過幾分變有錢人

    [Header("UI (選用)")]
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // 1. 讀取上一關傳過來的分數
        float finalScore = GameFlow.totalCash;

        // (選用) 顯示分數
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }

        // 2. 決定要生成哪個角色
        GameObject prefabToSpawn = null;

        if (finalScore >= scoreForRich)
        {
            // 高分：生成陣列第 2 個 (例如國王)
            if (characterPrefabs.Length > 2) prefabToSpawn = characterPrefabs[2];
        }
        else if (finalScore >= scoreForNormal)
        {
            // 中分：生成陣列第 1 個 (例如廚師)
            if (characterPrefabs.Length > 1) prefabToSpawn = characterPrefabs[1];
        }
        else
        {
            // 低分：生成陣列第 0 個 (例如乞丐)
            if (characterPrefabs.Length > 0) prefabToSpawn = characterPrefabs[0];
        }

        // 3. 實際生成角色
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject playerObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            // ⭐ 2. 找到場景中的攝影機腳本
            ThirdPersonCamera camScript = Camera.main.GetComponent<ThirdPersonCamera>();

            // ⭐ 3. 如果有找到，把新角色設為攝影機的目標
            if (camScript != null)
            {
                camScript.target = playerObj.transform;
            }
        }
    }

    // 提供一個按鈕讓玩家回到主選單或重玩
    public void GoToMainMenu()
    {
        // 離開結束場景時，才真正的重置分數
        GameFlow.ResetStatics();
        GameFlow.gameTime = 0f;

        SceneManager.LoadScene("StartMenu"); // 請改成你的主選單場景名
    }
}