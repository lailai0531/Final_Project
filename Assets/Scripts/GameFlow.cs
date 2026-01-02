using System;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow instance;

    public static int[] orderValue = new int[3];
    public static int[] plateValue = new int[3];
    public static float[] orderTimer = new float[3];

    public static int[] orderPrice = new int[3];

    public static Customer[] seatMap = new Customer[3];

    public static float gameTime = 0f;

    public static int plateNum = 0;
    public static float plateXpos = -0.75f;

    public static float emptyPlateNow = -3f;
    public static float totalCash = 0f;

    public Transform plateSelector;
    public Sprite[] orderPics;

    void Awake()
    {
        instance = this;
        ResetGameFlow();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            plateNum++;
            if (plateNum > 2)
            {
                plateNum = 0;
            }
            plateXpos = -0.75f + (plateNum * 2.55f);
        }

        if (plateSelector != null)
        {
            plateSelector.transform.position = new Vector3(plateXpos, 1.02f, 0.254f);
        }
    }

    public static void ResetGameFlow()
    {
        plateNum = 0;
        plateXpos = -0.75f;
        emptyPlateNow = -3f;
        totalCash = 0f;
        gameTime = 0f;

        for (int i = 0; i < 3; i++)
        {
            orderValue[i] = 0;
            plateValue[i] = 0;
            orderTimer[i] = 0f;
            orderPrice[i] = 0; 
            seatMap[i] = null;
        }
    }

    public static void ResetStatics()
    {
        totalCash = 0;
        gameTime = 0f;

        orderValue = new int[] { 0, 0, 0 };
        plateValue = new int[] { 0, 0, 0 };
        orderTimer = new float[] { 0, 0, 0 };

        orderPrice = new int[] { 0, 0, 0 };

        seatMap = new Customer[3];

        plateNum = 0;
        emptyPlateNow = -3;
    }
}