using UnityEngine;
using UnityEngine.AI; // 必須引用導航命名空間

public class GuestAI : MonoBehaviour
{
    public Transform counterLocation; // 在 Inspector 把櫃台位置拉進來
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 指令客人走到櫃台
        if (counterLocation != null)
        {
            agent.SetDestination(counterLocation.position);
        }
    }

    void Update()
    {
        // 檢查是否到達櫃台附近
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // 到達櫃台後的邏輯，例如：開始點餐
            Debug.Log("客人已抵達櫃台！");
            FaceCounter();
        }
    }

    void FaceCounter()
    {
        // 直接強行設定角度
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}