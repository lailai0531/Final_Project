using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMove : MonoBehaviour
{
    private int foodValue = 0;
    private MeshRenderer meat;
    private string stillcooking = "y"; 

    [Header("特效")]
    [SerializeField] private ParticleSystem smokeEffect;

    [Header("音效")]
    [SerializeField] private AudioSource grillAudio;
    [SerializeField] private AudioSource placeAudio;

    void Start()
    {
        meat = GetComponent<MeshRenderer>();

        if (smokeEffect != null)
        {
            smokeEffect.gameObject.SetActive(true);
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
        
        if (stillcooking != "n") { return; }

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

        transform.position = new Vector3(GameFlow.plateXpos, 1.8f, 0.2165146f);
        GameFlow.plateValue[GameFlow.plateNum] += foodValue;

        stillcooking = "done";

    }

    IEnumerator cookTimer()
    {
        yield return new WaitForSeconds(5);
        foodValue = 10;

        if (stillcooking == "y")
        {
            meat.material.color = new Color(.3f, .3f, .3f);
            stillcooking = "n"; 
            Debug.Log("肉熟了可拿取");
        }
    }
}