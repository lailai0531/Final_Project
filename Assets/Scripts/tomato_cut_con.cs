using UnityEngine;

[RequireComponent(typeof(AudioSource))] // 這行會強制確保物件上有 AudioSource 元件
public class tomato_cut_con : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("音效設定")]
    public AudioClip chopClip; // 請在 Inspector 把切菜的聲音拉進來

    private void Start()
    {
        // 取得自身的 AudioSource
        audioSource = GetComponent<AudioSource>();

        // 設定並播放音效
        if (chopClip != null)
        {
            audioSource.clip = chopClip;
            audioSource.Play(); // 開始播放
        }
    }

    private void OnMouseDown()
    {
        // 當點擊這個「已經切開的番茄」時
        if (audioSource.isPlaying)
        {
            Debug.Log("停止音效");
            audioSource.Stop(); // 停止播放
        }
    }
}