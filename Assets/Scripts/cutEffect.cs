using UnityEngine;

public class cutEffect : MonoBehaviour
{
    [Header("音效替身 (新版)")]
    // 這裡改用 GameObject，對應你的 CutSFX Prefab
    [SerializeField] private AudioSource chopSFX;


    private void OnMouseDown()
    {
        chopSFX.Play();
    }
}