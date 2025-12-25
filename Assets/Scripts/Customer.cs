using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("UI 連結")]
    public Image orderImage;
    public Slider timerSlider;


    private int mySeatIndex = -1;
    private float myTime = 30f;

    private int[] menuCodes = { 111111, 111101, 011110, 110001 };

    public void Initialize(int seatIndex)
    {
        mySeatIndex = seatIndex;

        GameFlow.seatMap[seatIndex] = this;

        int randomPick = Random.Range(0, menuCodes.Length);
        int chosenCode = menuCodes[randomPick];

        GameFlow.orderValue[seatIndex] = chosenCode;
        GameFlow.orderTimer[seatIndex] = myTime;

        if (GameFlow.instance != null)
        {
            orderImage.sprite = GameFlow.instance.orderPics[randomPick];
        }
        timerSlider.maxValue = myTime;
        timerSlider.value = myTime;
    }

    void Update()
    {
        if (mySeatIndex == -1) return;


        GameFlow.orderTimer[mySeatIndex] -= Time.deltaTime;
        timerSlider.value = GameFlow.orderTimer[mySeatIndex];


        if (GameFlow.orderTimer[mySeatIndex] <= 0)
        {
            Leave(false);
        }
    }


    public void Leave(bool isHappy)
    {

        GameFlow.seatMap[mySeatIndex] = null;
        GameFlow.orderValue[mySeatIndex] = 0;

        if (isHappy)
        {
            Debug.Log("好吃！");

        }
        else
        {
            Debug.Log("生氣！");
        }

        Destroy(gameObject);
    }
}
