using UnityEngine;
using UnityEngine.SceneManagement; // 1. 【重要】一定要有這行

public class RestartManager : MonoBehaviour
{
    // 這是給按鈕呼叫的函式
    public void ClickRestart()
    {
        // 2. 先重置數據
        GameFlow.ResetStatics();

        // 3. 再重新載入目前的場景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
