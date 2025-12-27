using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMove : MonoBehaviour
{
    private int foodValue = 0;
    private MeshRenderer meat;
    private string stillcooking = "y";

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokeEffect;

    [Header("Audio")]
    [SerializeField] private AudioSource grillAudio;
    [SerializeField] private AudioSource placeAudio;

    void Start()
    {
        meat = GetComponent<MeshRenderer>();

        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        if (grillAudio != null)
        {
            grillAudio.loop = true;
            grillAudio.Play();
        }

        StartCoroutine(cookTimer());
    }

    public void Interact()
    {
        if (stillcooking == "y") { return; }

        if (placeAudio != null)
        {
            placeAudio.PlayOneShot(placeAudio.clip);
        }

        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }

        if (grillAudio != null)
        {
            grillAudio.Stop();
        }

        GetComponent<Transform>().position = new Vector3(GameFlow.plateXpos, 1.8f, 0.2165146f);
        GameFlow.plateValue[GameFlow.plateNum] += foodValue;

        stillcooking = "y";
    }

    IEnumerator cookTimer()
    {
        yield return new WaitForSeconds(5);
        foodValue = 10;

        if (stillcooking == "y")
        {
            meat.material.color = new Color(.3f, .3f, .3f);
            stillcooking = "n";
        }
    }
}
