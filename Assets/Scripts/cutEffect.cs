using UnityEngine;

public class cutEffect : MonoBehaviour
{
    [Header("­µ®Ä´À¨­")]
    [SerializeField] private AudioSource chopSFX;


    private void OnMouseDown()
    {
        chopSFX.Play();
    }
}