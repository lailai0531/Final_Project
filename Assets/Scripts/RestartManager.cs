using UnityEngine;
using UnityEngine.SceneManagement; 

public class RestartManager : MonoBehaviour
{
    public void ClickRestart()
    {
        GameFlow.ResetStatics();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
