using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class tomato_cut_con : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("音效設定")]
    public AudioClip chopClip; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (chopClip != null)
        {
            audioSource.clip = chopClip;
            audioSource.Play(); 
        }
    }

    private void OnMouseDown()
    {
        if (audioSource.isPlaying)
        {
            Debug.Log("停止音效");
            audioSource.Stop(); 
        }
    }
}