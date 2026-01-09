using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickplace : MonoBehaviour
{
    public Transform cloneObj;
    public int foodValue;

    public float thickness = 0.15f;
    public static float currentHeight = 1.2f;


    [Header("Audio")]
    [SerializeField] private AudioSource placeAudio;

    void Start()
    {
    }

    void Update()
    {
    }

    public void Interact()
    {
        Debug.Log("點到的物件是：" + gameObject.name);
        if (placeAudio != null)
        {
            placeAudio.PlayOneShot(placeAudio.clip);
        }

        Vector3 spawnPos = new Vector3(GameFlow.plateXpos, currentHeight, 0.2165146f);

        if (gameObject.name == "bunbottom")
        {
            spawnPos = new Vector3(GameFlow.plateXpos, 1.15f, 0.2165146f);
            currentHeight += thickness;
        }
        else if (gameObject.name == "buntop")
        {
            spawnPos = new Vector3(GameFlow.plateXpos, 2.5f, 0.2165146f);
            currentHeight += thickness;
        }
        else
        {
            spawnPos = new Vector3(GameFlow.plateXpos, 1.4f, 0.2165146f);
            currentHeight += thickness;
        }

        Instantiate(cloneObj, spawnPos, cloneObj.rotation);
        if (!gameObject.name.Contains("tomato"))
        {
            GameFlow.totalCash -= 1;
        }

        if (gameObject.name.Contains("tomato"))
        {
            Destroy(gameObject);
        }

        GameFlow.plateValue[GameFlow.plateNum] += foodValue;
        Debug.Log(GameFlow.plateValue[GameFlow.plateNum] + " " + GameFlow.orderValue[GameFlow.plateNum]);
    }
}