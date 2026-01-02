using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("客人")]
    public GameObject[] customerPrefabs; 

    [Header("生成點與櫃台點")]
    public Transform spawnPoint;    
    public Transform[] counterPoints; 

    public float checkInterval = 2f;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
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
        if (customerPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameObject selectedPrefab = customerPrefabs[randomIndex];

        GameObject newGuest = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        Customer guestScript = newGuest.GetComponent<Customer>();
        if (guestScript != null)
        {
            guestScript.Initialize(counterPoints[seatIndex], seatIndex);
        }
    }
}

