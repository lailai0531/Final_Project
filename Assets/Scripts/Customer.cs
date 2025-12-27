using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; // 【重要】引用導航系統

// 必須有 NavMeshAgent 組件，如果沒有會自動加
[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [Header("UI 連結")]
    public Image orderImage;       // 頭上的訂單圖片
    public Slider timerSlider;     // 頭上的時間條
    public GameObject uiCanvas;    // 頭上的 Canvas (一開始先隱藏)

    [Header("狀態設定")]
    private NavMeshAgent agent;
    private int mySeatIndex = -1;  // 我是第幾號桌的客人
    private bool hasArrived = false; // 是否已經抵達櫃台

    // 【新增】用來控制變色的圖片元件
    private Image sliderFillImage;

    // 菜單代碼 (對應 GameFlow 的 Sprite 順序)
    private int[] menuCodes = { 111111, 111101, 011110, 110001 };
    private float myTime = 30f;    // 訂單時間

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // 為了防止走路時頭上的 UI 亂轉，可以禁止 Agent 控制旋轉 (選用)
        // agent.updateRotation = false; 
    }

    // === 初始化函式 (由生成器呼叫) ===
    public void Initialize(Transform targetPoint, int seatIndex)
    {
        mySeatIndex = seatIndex;

        // 1. 登記座位到 GameFlow
        GameFlow.seatMap[seatIndex] = this;

        // 2. 設定導航目的地
        agent.SetDestination(targetPoint.position);

        // 3. 一開始先隱藏訂單 UI
        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    void Update()
    {
        if (mySeatIndex == -1) return;
        // 階段一：走路中
        if (!hasArrived)
        {
            // 檢查是否抵達目的地 (距離小於 0.5)
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                ArriveAtCounter();
            }
        }
        // 階段二：抵達後，開始倒數計時
        else
        {
            // 防呆：確保座位索引正確
            if (mySeatIndex == -1) return;

            // 更新 GameFlow 的計時器
            GameFlow.orderTimer[mySeatIndex] -= Time.deltaTime;

            float currentTime = GameFlow.orderTimer[mySeatIndex];

            // 更新頭上 UI
            if (timerSlider != null)
            {
                timerSlider.value = currentTime;

                // 【新增】變色邏輯
                // 必須先確保有抓到 sliderFillImage
                if (sliderFillImage != null)
                {
                    // 計算比例：剩餘時間 / 總時間
                    float ratio = currentTime / myTime;

                    // 如果剩下不到 30% (0.3)，變紅色
                    if (ratio <= 0.3f)
                    {
                        sliderFillImage.color = Color.red;
                    }
                    else
                    {
                        sliderFillImage.color = Color.green; // 平常是綠色
                    }
                }
            }

            // 檢查時間到
            if (currentTime <= 0)
            {
                Leave(false); // 生氣離開
            }
        }
    }

    // === 抵達櫃台要做的事 ===
    void ArriveAtCounter()
    {
        hasArrived = true;

        // 【關鍵修改 1】關閉導航代理的旋轉控制
        // 這樣它就不會跟你搶控制權，客人就會乖乖定住
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.velocity = Vector3.zero; // 順便強制煞車，防止滑步
        }

        // 1. 面向鏡頭 (抬頭看你)
        if (Camera.main != null)
        {
            // 因為上面關掉了 updateRotation，這行 LookAt 現在會永久生效了
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // 2. 顯示頭上 UI
        if (uiCanvas != null) uiCanvas.SetActive(true);

        // 3. 產生隨機訂單
        GenerateRandomOrder();
    }



    // 2. 【新增】這個函式會在動畫播完後執行，強迫他看著你
    void LateUpdate()
    {
        // 確保只有抵達後才執行 LookAt
        if (hasArrived && Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);

            // 鎖定 X 和 Z 軸旋轉，只讓怪轉 Y 軸 (水平轉)，不然怪會仰頭看你很怪，也會卡
            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, euler.y, 0);
        }
    }






    public bool IsArrived()
    {
        return hasArrived;
    }

    void GenerateRandomOrder()
    {
        // 【防呆修正 1】檢查座位編號是否有效
        // 如果是 -1 (還沒初始化) 或者 超過 GameFlow 的陣列長度 (例如只有3個位子卻傳來3號)
        if (mySeatIndex < 0 || mySeatIndex >= GameFlow.orderValue.Length)
        {
            Debug.LogError("嚴重錯誤：座位編號無效！目前索引: " + mySeatIndex);
            return; // 直接中斷，保護程式不崩潰
        }

        // 【防呆修正 2】檢查菜單陣列是否為空
        if (menuCodes == null || menuCodes.Length == 0)
        {
            Debug.LogError("嚴重錯誤：menuCodes 沒設定或是空的！");
            return;
        }

        // 隨機選菜
        int randomPick = Random.Range(0, menuCodes.Length);
        int chosenCode = menuCodes[randomPick];

        // 寫入 GameFlow (因為上面有檢查過 mySeatIndex，這裡絕對安全了)
        GameFlow.orderValue[mySeatIndex] = chosenCode;
        GameFlow.orderTimer[mySeatIndex] = myTime;

        // 更新圖片
        if (GameFlow.instance != null && orderImage != null)
        {
            // 也要確保 GameFlow 圖片陣列夠長
            if (randomPick < GameFlow.instance.orderPics.Length)
            {
                orderImage.sprite = GameFlow.instance.orderPics[randomPick];
            }
        }

        // 初始化 Slider
        if (timerSlider != null)
        {
            timerSlider.maxValue = myTime;
            timerSlider.value = myTime;

            // 抓取 Slider 裡面的 Fill Image 並重置顏色
            if (timerSlider.fillRect != null)
            {
                sliderFillImage = timerSlider.fillRect.GetComponent<Image>();

                // 一開始設回綠色
                if (sliderFillImage != null)
                {
                    sliderFillImage.color = Color.green;
                }
            }
        }
    }

    // === 離開邏輯 ===
    public void Leave(bool isHappy)
    {
        // 清除資料
        if (mySeatIndex != -1)
        {
            GameFlow.seatMap[mySeatIndex] = null;
            GameFlow.orderValue[mySeatIndex] = 0;
        }

        // 簡單做法：直接刪除
        Destroy(gameObject);
    }
}
