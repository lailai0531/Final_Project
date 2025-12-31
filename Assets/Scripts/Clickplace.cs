using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class clickplace : MonoBehaviour
{
    public Transform cloneObj;
    public int foodValue;

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
        Debug.Log("【除錯】你點到的物件名字是：" + gameObject.name );
        if (placeAudio != null)
        {
            placeAudio.PlayOneShot(placeAudio.clip);
        }

        if (gameObject.name == "bunbottom")
        {
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
            GameFlow.totalCash -= 1;

        }
        if (gameObject.name == "buntop")
        {
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
            GameFlow.totalCash -= 1;

        }
        if (gameObject.name == "lettuce")
        {
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
            GameFlow.totalCash -= 1;

        }
        if (gameObject.name == "cheese")
        {
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
            GameFlow.totalCash -= 1;

        }
        if (gameObject.name.Contains( "tomato"))
        {
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
            GameFlow.totalCash -= 1;
            Destroy(gameObject);

        }

        GameFlow.plateValue[GameFlow.plateNum] += foodValue;
        Debug.Log(GameFlow.plateValue[GameFlow.plateNum] + " " + GameFlow.orderValue[GameFlow.plateNum]);
    }
}
