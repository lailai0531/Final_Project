using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServePlate : MonoBehaviour
{
    public int thisPlate = 0;

    void Start() { }

    void Update()
    {
        // 1. 送餐 (Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameFlow.plateNum == thisPlate) Interact();
        }

        // 2. 倒掉 (1)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GameFlow.plateNum == thisPlate) ClearPlate();
        }
    }

    // ==========================================
    //  送餐邏輯
    // ==========================================
    public void Interact()
    {
        // 1. 確保目前選到的是這個盤子
        if (GameFlow.plateNum != thisPlate) return;

        // 2. 檢查盤子是不是空的
        if (GameFlow.plateValue[thisPlate] == 0) return;

        // ----------------------------------------------------
        // 3. 【新增】檢查客人狀態 (關鍵邏輯)
        // ----------------------------------------------------

        // 先從座位表抓出這個位子的客人
        Customer currentCustomer = GameFlow.seatMap[thisPlate];

        // 情況 A：根本沒有客人 (是 null)
        if (currentCustomer == null)
        {
            Debug.Log("這裡沒有客人！");
            return; // 禁止送餐
        }

        // 情況 B：有客人，但還在走路 (還沒 Arrived)
        // 呼叫我們剛剛在 Customer 寫的新函式
        if (!currentCustomer.IsArrived())
        {
            Debug.Log("客人還沒走到櫃台，請稍等！");
            return; // 禁止送餐
        }
        // ----------------------------------------------------

        // --- 核心出餐邏輯 (原本的程式碼) ---
        bool isRight = (GameFlow.orderValue[thisPlate] == GameFlow.plateValue[thisPlate]);
        float timeLeft = GameFlow.orderTimer[thisPlate];

        StartCoroutine(ProcessServing(isRight, timeLeft));
    }

    // ... (ClearPlate 和 ProcessServing 保持原本的樣子不用動) ...
    public void ClearPlate()
    {
        if (GameFlow.plateValue[thisPlate] == 0) return;
        GameFlow.plateValue[thisPlate] = 0;
        Debug.Log(thisPlate + " 號盤子的東西被倒掉了！");
        StartCoroutine(ShowClearEffect());
    }

    IEnumerator ShowClearEffect()
    {
        GameFlow.emptyPlateNow = transform.position.x;
        yield return new WaitForSeconds(0.2f);
        GameFlow.emptyPlateNow = -3;
    }

    IEnumerator ProcessServing(bool isRight, float timeLeft)
    {
        GameFlow.emptyPlateNow = transform.position.x;
        yield return new WaitForSeconds(0.2f);
        GameFlow.emptyPlateNow = -3;

        if (isRight)
        {
            GameFlow.totalCash += timeLeft * 10;
            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);
        }
        else
        {
            GameFlow.totalCash -= 50;
            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);
            Debug.Log("做錯了！");
        }
        GameFlow.plateValue[thisPlate] = 0;
    }
}