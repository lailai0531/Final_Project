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
        ResetGameFlow();   // ⭐ 確保每次 Instantiate 都是乾淨的
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 【抓兇手專用 Debug】
            // 這行會印出是「哪個物件」在執行這段程式碼
            // 還有它的「唯一 ID」，如果 ID 不同，代表有兩個分身！
            //Debug.Log("執行者: " + gameObject.name + " | ID: " + GetInstanceID() + " | 時間: " + Time.time);

            plateNum++;
            if (plateNum > 2)
            {
                plateNum = 0;
            }
            plateXpos = -0.75f + (plateNum * 2.55f);
        }

        if (plateSelector != null)
        {
            // 建議用 Lerp 比較平滑，這裡先維持原本的
            plateSelector.transform.position = new Vector3(plateXpos, 1.02f, 0.254f);
        }
    }
    // =====================
    // Reset (給 Restart 用)
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
            seatMap[i] = null;
        }
    }
    // 【新增】這個函式專門用來把所有資料歸零
    public static void ResetStatics()
    {
        // 1. 金錢歸零
        totalCash = 0;
        gameTime = 0f;

        // 2. 陣列清空 (建立新的空陣列)
        orderValue = new int[] { 0, 0, 0 };
        plateValue = new int[] { 0, 0, 0 };
        orderTimer = new float[] { 0, 0, 0 };

        // 3. 座位清空
        seatMap = new Customer[3];

        // 4. 重置盤子指標
        plateNum = 0;
        // plateXpos 不用在這裡重置，因為 Awake 已經會處理它

        emptyPlateNow = -3;

        //Debug.Log("資料已全部重置！");
    }
}