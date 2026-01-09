using UnityEngine;

public class TomatoBar : MonoBehaviour
{
    [Header("設定")]
    public GameObject boardTomatoPrefab;
    public Transform boardPosition;

    [Header("音效替身")]
    [SerializeField] private AudioSource placeAudio;


    private void OnMouseDown()
    {
        if (!MainController.isGameRunning) return;
        if (placeAudio != null)
        {
            placeAudio.Play();
        }
        else
        {
            Debug.LogWarning("沒聲音");
        }

        if (boardTomatoPrefab != null)
        {
            GameFlow.totalCash -= 1;
            Vector3 spawnPos = (boardPosition != null) ? boardPosition.position : new Vector3(0, 1.75f, -1.8f);
            Instantiate(boardTomatoPrefab, spawnPos, Quaternion.identity);
            Debug.Log("生成新番茄");
        }

        // Destroy(gameObject); 
    }
}