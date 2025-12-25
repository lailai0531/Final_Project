using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints;

    public float spawnInterval = 3f;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnCustomer();
            timer = 0;
        }
    }

    void TrySpawnCustomer()
    {

        for (int i = 0; i < 3; i++)
        {

            if (GameFlow.seatMap[i] == null)
            {
                Spawn(i);
                return;
            }
        }
    }

    void Spawn(int seatIndex)
    {

        GameObject newCustomer = Instantiate(customerPrefab, spawnPoints[seatIndex].position, Quaternion.identity);


        newCustomer.GetComponent<Customer>().Initialize(seatIndex);
    }
}
