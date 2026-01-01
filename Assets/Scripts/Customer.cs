using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [Header("UI 連結")]
    public Image orderImage;
    public Slider timerSlider;
    public GameObject uiCanvas;

    [Header("狀態設定")]
    private NavMeshAgent agent;
    private int mySeatIndex = -1;
    private bool hasArrived = false;

    private Image sliderFillImage;

    // 菜單代碼 (對應 GameFlow 的 Sprite 順序)
    private int[] menuCodes = { 111111, 111101, 011110, 110001 };

    // ⭐【新增】菜單價格 (對應上面的 menuCodes)
    // 例如: 111111(漢堡)賣120元, 111101(薯條)賣80元...請依序填入
    private int[] menuPrices = { 100, 60, 60, 40 };

    private float myTime = 50f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(Transform targetPoint, int seatIndex)
    {
        mySeatIndex = seatIndex;
        GameFlow.seatMap[seatIndex] = this;
        agent.SetDestination(targetPoint.position);

        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    void Update()
    {
        if (mySeatIndex == -1) return;

        if (!hasArrived)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                ArriveAtCounter();
            }
        }
        else
        {
            if (mySeatIndex == -1) return;

            GameFlow.orderTimer[mySeatIndex] -= Time.deltaTime;
            float currentTime = GameFlow.orderTimer[mySeatIndex];

            if (timerSlider != null)
            {
                timerSlider.value = currentTime;

                if (sliderFillImage != null)
                {
                    float ratio = currentTime / myTime;
                    if (ratio <= 0.3f) sliderFillImage.color = Color.red;
                    else sliderFillImage.color = Color.green;
                }
            }

            if (currentTime <= 0)
            {
                Leave(false);
            }
        }
    }

    void ArriveAtCounter()
    {
        hasArrived = true;

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.velocity = Vector3.zero;
        }

        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (uiCanvas != null) uiCanvas.SetActive(true);

        GenerateRandomOrder();
    }

    void LateUpdate()
    {
        if (hasArrived && Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
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
        if (mySeatIndex < 0 || mySeatIndex >= GameFlow.orderValue.Length) return;
        if (menuCodes == null || menuCodes.Length == 0) return;

        int randomPick = Random.Range(0, menuCodes.Length);
        int chosenCode = menuCodes[randomPick];

        // 寫入 GameFlow
        GameFlow.orderValue[mySeatIndex] = chosenCode;
        GameFlow.orderTimer[mySeatIndex] = myTime;

        // ⭐【新增】寫入價格
        // 防呆：確認 randomPick 有在 menuPrices 範圍內
        if (randomPick < menuPrices.Length)
        {
            GameFlow.orderPrice[mySeatIndex] = menuPrices[randomPick];
        }
        else
        {
            GameFlow.orderPrice[mySeatIndex] = 50; // 預設低消
        }

        // 更新圖片
        if (GameFlow.instance != null && orderImage != null)
        {
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

            if (timerSlider.fillRect != null)
            {
                sliderFillImage = timerSlider.fillRect.GetComponent<Image>();
                if (sliderFillImage != null) sliderFillImage.color = Color.green;
            }
        }
    }

    public void Leave(bool isHappy)
    {
        if (mySeatIndex != -1)
        {
            GameFlow.seatMap[mySeatIndex] = null;
            GameFlow.orderValue[mySeatIndex] = 0;
            // ⭐【新增】離開時清除價格
            GameFlow.orderPrice[mySeatIndex] = 0;
            if(isHappy == false)
            {
                GameFlow.totalCash -= 60;

            }
        }

        Destroy(gameObject);
    }
}