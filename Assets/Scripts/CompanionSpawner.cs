using UnityEngine;
using System.Collections.Generic;
public class CompanionSpawner : MonoBehaviour
{
    [System.Serializable] //這行一定要加，Unity 編輯器才看得到
    public class SpawnItem
    {
        public string name = "物體名稱"; // 方便你在編輯器辨識用
        public GameObject prefab;       // 要生成的東西
        public Vector3 offset;          // 位置偏移 (X, Y, Z)
        public Vector3 rotationOffset;  // 旋轉偏移 (例如車子要轉90度才不會撞牆)
    }

    [Header("要生成的物體清單")]
    // 這裡變成了清單，你可以在 Inspector 裡設定 Size 為 2, 3, 4...
    public List<SpawnItem> companions = new List<SpawnItem>();

    [Header("全域設定")]
    public bool useLocalPosition = true; // 打勾=跟隨角色面向；不勾=固定世界座標

    void Start()
    {
        SpawnAll();
    }

    void SpawnAll()
    {
        // 跑迴圈，把清單裡每一個東西都生出來
        foreach (var item in companions)
        {
            if (item.prefab == null) continue;

            // 1. 計算位置
            Vector3 spawnPos;
            Quaternion spawnRot;

            if (useLocalPosition)
            {
                // 相對座標：角色轉身，這些東西也會跟著轉到對應位置
                spawnPos = transform.TransformPoint(item.offset);

                // 相對旋轉：角色的旋轉 + 物體自己的旋轉偏移
                spawnRot = transform.rotation * Quaternion.Euler(item.rotationOffset);
            }
            else
            {
                // 世界座標：固定不動
                spawnPos = transform.position + item.offset;
                spawnRot = Quaternion.Euler(item.rotationOffset);
            }

            // 2. 生成
            Instantiate(item.prefab, spawnPos, spawnRot);
        }
    }
}