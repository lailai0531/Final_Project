using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickplace : MonoBehaviour
{
    public Transform cloneObj;
    public int foodValue;

    void Start()
    {

    }
    void Update()
    {

    }
    private void OnMouseDown()
    {
        if (gameObject.name == "bunbottom")
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
        if (gameObject.name == "buntop")
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
        if (gameObject.name == "lettuce")
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
        if (gameObject.name == "cheese")
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
        if (gameObject.name == "tomato")
            Instantiate(cloneObj, new Vector3(GameFlow.plateXpos, 2.2f, 0.2165146f), cloneObj.rotation);
        GameFlow.plateValue[GameFlow.plateNum] += foodValue;
        Debug.Log(GameFlow.plateValue[GameFlow.plateNum] + " " + GameFlow.orderValue[GameFlow.plateNum]);
    }
}
