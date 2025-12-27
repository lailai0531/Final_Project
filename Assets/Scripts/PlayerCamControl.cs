using UnityEngine;

public class SoftCameraLook : MonoBehaviour
{
    [Header("視角移動範圍 (度)")]
    public float maxTiltX = 15f; // 上下能看多寬 (抬頭低頭)
    public float maxTiltY = 20f; // 左右能看多寬 (轉頭)

    [Header("平滑度")]
    public float smoothTime = 5f; // 數值越小反應越快，越大越滑順

    // 紀錄相機一開始的基準角度 (例如原本是稍微往下看)
    private Quaternion initialRotation;

    void Start()
    {
        // 1. 記住遊戲開始時相機原本的角度
        // 這樣你原本在編輯器裡擺好的角度就是「正中心」
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // 2. 取得滑鼠在螢幕上的位置 (0 ~ 1)
        // (0,0) 是左下角，(1,1) 是右上角，(0.5, 0.5) 是正中間
        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;

        // 3. 把 0~1 轉換成 -1 ~ 1 (這樣中心點才是 0)
        // 數學：(0.5 - 0.5) * 2 = 0
        float inputX = (mouseX - 0.5f) * 2;
        float inputY = (mouseY - 0.5f) * 2;

        // 4. 計算目標角度
        // inputY 控制左右轉 (Y軸)，inputX 控制上下轉 (X軸)
        // 注意：上下轉通常要反向 (滑鼠往上是抬頭，角度是負的)
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-inputY * maxTiltX, inputX * maxTiltY, 0f);

        // 5. 平滑移動過去
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothTime);
    }
}
