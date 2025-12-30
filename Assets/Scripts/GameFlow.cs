using System;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow instance;

    // =====================
    // Static Game Data
    // =====================
    public static int[] orderValue = new int[3];
    public static int[] plateValue = new int[3];
    public static float[] orderTimer = new float[3];

    // ⭐【新增】用來存每一桌訂單的「價格」
    public static int[] orderPrice = new int[3];

    public static Customer[] seatMap = new Customer[3];

    public static float gameTime = 0f;

    public static int plateNum = 0;
    public static float plateXpos = -0.75f;

    public static float emptyPlateNow = -3f;
    public static float totalCash = 0f;

    // =====================
    // Inspector References
    // =====================
    public Transform plateSelector;
    public Sprite[] orderPics;

    // =====================
    // Unity Lifecycle
    // =====================
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

    // =====================
    // Reset
    // =====================
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
            orderPrice[i] = 0; // ⭐ 重置時也要歸零
            seatMap[i] = null;
        }
    }

    public static void ResetStatics()
    {
        // 1. 金錢歸零
        totalCash = 0;
        gameTime = 0f;

        // 2. 陣列清空
        orderValue = new int[] { 0, 0, 0 };
        plateValue = new int[] { 0, 0, 0 };
        orderTimer = new float[] { 0, 0, 0 };

        // ⭐【新增】價格陣列也要重置
        orderPrice = new int[] { 0, 0, 0 };

        // 3. 座位清空
        seatMap = new Customer[3];

        // 4. 重置盤子指標
        plateNum = 0;
        emptyPlateNow = -3;
    }
}