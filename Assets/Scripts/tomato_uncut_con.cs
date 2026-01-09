using UnityEngine;
using System.Collections;

public class tomato_uncut_con : MonoBehaviour
{
    [Header("設定")]
    public Transform cutTomatoPrefab;
    public float delayTime = 0.5f;

    [Header("特效")]
    public GameObject particlePrefab;

    [Header("音效")]
    [SerializeField] private AudioSource chopSFX;

    private bool isCutting = false;

    private void OnMouseDown()
    {
        if (!MainController.isGameRunning) return;

        if (isCutting) return;

        StartCoroutine(ProcessCut());
    }

    IEnumerator ProcessCut()
    {
        isCutting = true;

        if (particlePrefab != null)
        {
            GameObject vfx = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2.0f);
        }

        if (chopSFX != null && chopSFX.clip != null)
        {
            AudioSource.PlayClipAtPoint(chopSFX.clip, Camera.main.transform.position, 1.0f);
        }

        yield return new WaitForSeconds(delayTime);

        if (cutTomatoPrefab != null)
        {
            Instantiate(cutTomatoPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}