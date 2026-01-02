using UnityEngine;
using System.Collections;

public class BurgerEndingBehavior : MonoBehaviour
{
    [Header("模型設定")]
    public GameObject burgerOnPlate; 
    public GameObject emptyPlate;    

    [Header("掉落設定 (新功能)")]
    public float dropHeight = 5.0f;     
    public float dropDuration = 0.5f;   
    public AudioClip landSound;        

    [Header("鏡頭運鏡設定")]
    public Transform cameraEndAnchor;   
    public float moveDuration = 1.0f;   

    [Header("吃掉設定")]
    public AudioSource audioSource;
    public AudioClip biteSound;        

    void Start()
    {
        
        if (burgerOnPlate != null) burgerOnPlate.SetActive(true);
        if (emptyPlate != null) emptyPlate.SetActive(false);

        
        StartCoroutine(MainSequence());
    }

    IEnumerator MainSequence()
    {
        
        Vector3 landPosition = transform.position;

        Vector3 skyPosition = landPosition + Vector3.up * dropHeight;
        transform.position = skyPosition;


        float timer = 0f;
        while (timer < dropDuration)
        {
            timer += Time.deltaTime;
            float t = timer / dropDuration;
            t = t * t; 

            transform.position = Vector3.Lerp(skyPosition, landPosition, t);
            yield return null;
        }

        transform.position = landPosition;

        if (audioSource != null && landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }

        yield return new WaitForSeconds(0.3f);


        Camera mainCam = Camera.main;
        if (mainCam != null && cameraEndAnchor != null)
        {
            var tpCamera = mainCam.GetComponent<ThirdPersonCamera>();
            if (tpCamera != null) tpCamera.enabled = false;

            Vector3 startCamPos = mainCam.transform.position;
            Quaternion startCamRot = mainCam.transform.rotation;

            timer = 0f;
            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                t = Mathf.SmoothStep(0, 1, t); 

                mainCam.transform.position = Vector3.Lerp(startCamPos, cameraEndAnchor.position, t);
                mainCam.transform.rotation = Quaternion.Slerp(startCamRot, cameraEndAnchor.rotation, t);

                yield return null;
            }
        }


        yield return new WaitForSeconds(4.5f); 

        if (audioSource != null && biteSound != null)
        {
            audioSource.PlayOneShot(biteSound);
        }

        if (burgerOnPlate != null) burgerOnPlate.SetActive(false);
        if (emptyPlate != null) emptyPlate.SetActive(true);

        Debug.Log("演出結束：掉落 -> 運鏡 -> 吃掉");
    }
}