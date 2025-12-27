using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("多種客人造型 Prefab")]
    public GameObject[] customerPrefabs; // 拖入男生、女生、外星人...

    [Header("生成點與櫃台點")]
    public Transform spawnPoint;    // 客人出生的地方 (例如門口)
    public Transform[] counterPoints; // 3個櫃台前的站立點 (目標點)

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
        // 檢查 0, 1, 2 號座位有沒有空位
        for (int i = 0; i < 3; i++)
        {
            if (GameFlow.seatMap[i] == null)
            {
                Spawn(i);
                return; // 一次只生一個
            }
        }
    }

    void Spawn(int seatIndex)
    {
        if (customerPrefabs.Length == 0) return;

        // 1. 隨機選一個造型
        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameObject selectedPrefab = customerPrefabs[randomIndex];

        // 2. 在門口生成
        GameObject newGuest = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // 3. 取得 Customer 腳本並初始化
        Customer guestScript = newGuest.GetComponent<Customer>();
        if (guestScript != null)
        {
            // 【關鍵】告訴客人：你的目標是 counterPoints[i]，座位是 i
            guestScript.Initialize(counterPoints[seatIndex], seatIndex);
        }
    }
}

