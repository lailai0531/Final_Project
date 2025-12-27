using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class burger_uncooked_con : MonoBehaviour
{
    public Transform cloneObj;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Interact()
    {
        if (gameObject.name == "burger_uncooked")
            Instantiate(cloneObj, new Vector3(-0.0500000007f, 1.75600004f, -1.78999996f), cloneObj.rotation);
    }
}
