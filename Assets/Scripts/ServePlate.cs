using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // ⭐ 確保一定有 AudioSource
public class ServePlate : MonoBehaviour
{
    public int thisPlate = 0;

    [Header("Audio")]
    [SerializeField] private AudioSource serveSuccessAudio;
    [SerializeField] private AudioSource serveFailAudio;



    void Start()
    {

    }

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

    public void Interact()
    {
        if (GameFlow.plateNum != thisPlate) return;
        if (GameFlow.plateValue[thisPlate] == 0) return;

        Customer currentCustomer = GameFlow.seatMap[thisPlate];

        if (currentCustomer == null)
        {
            Debug.Log("這裡沒有客人！");
            return;
        }

        if (!currentCustomer.IsArrived())
        {
            Debug.Log("客人還沒走到櫃台，請稍等！");
            return;
        }

        bool isRight = (GameFlow.orderValue[thisPlate] == GameFlow.plateValue[thisPlate]);
        float timeLeft = GameFlow.orderTimer[thisPlate];

        StartCoroutine(ProcessServing(isRight, timeLeft));
    }

    public void ClearPlate()
    {
        if (GameFlow.plateValue[thisPlate] == 0) return;
        GameFlow.plateValue[thisPlate] = 0;
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
            // ⭐ 播放成功音效
            serveSuccessAudio.Play();

            int basePrice = GameFlow.orderPrice[thisPlate];
            float tip = timeLeft * 1.5f;
            GameFlow.totalCash += (basePrice + tip);

            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);
        }
        else
        {
            // ⭐ 播放失敗音效
            serveFailAudio.Play();

            GameFlow.totalCash -= 50;

            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);

            Debug.Log("做錯了！");
        }

        GameFlow.plateValue[thisPlate] = 0;
        GameFlow.orderPrice[thisPlate] = 0;
    }
}
