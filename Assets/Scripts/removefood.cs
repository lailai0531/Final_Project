using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class removefood : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        if ((GameFlow.emptyPlateNow > transform.position.x - .4f) && (GameFlow.emptyPlateNow < transform.position.x + .4f))
        {
            Destroy(gameObject);
        }
    }
}
