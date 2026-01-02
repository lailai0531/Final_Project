using UnityEngine;

public class tomato_uncut_con : MonoBehaviour
{
    [Header("設定")]
    public Transform cutTomatoPrefab; 

    [Header("特效")]
    public GameObject particlePrefab; 

    [Header("音效")]
    [SerializeField] private AudioSource chopSFX;

    private void OnMouseDown()
    {
        if (!MainController.isGameRunning) return;

        Debug.Log("切菜動作執行");

        if (cutTomatoPrefab == null) return;

        Instantiate(cutTomatoPrefab, transform.position, transform.rotation);

        if (particlePrefab != null)
        {
            GameObject vfx = Instantiate(particlePrefab, transform.position, Quaternion.identity);

            Destroy(vfx, 2.0f);
        }

        if (chopSFX != null && chopSFX.clip != null)
        {
            AudioSource.PlayClipAtPoint(chopSFX.clip, Camera.main.transform.position, 1.0f);
        }

        Destroy(gameObject);
    }
}