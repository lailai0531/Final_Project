using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMove : MonoBehaviour
{
    private int foodValue = 0;
    private MeshRenderer meat;
    private string stillcooking = "y";

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokeEffect; // 在 Inspector 把粒子系統拉進來

    void Start()
    {
        meat = GetComponent<MeshRenderer>();

        // 開始煎肉時播放煙霧
        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        StartCoroutine(cookTimer());
    }

    private void OnMouseDown()
    {
        // 如果還在煮，點擊無效
        if (stillcooking == "y") { return; }

        // 拿走肉時，關閉煙霧
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }

        // 移動到盤子
        GetComponent<Transform>().position = new Vector3(GameFlow.plateXpos, 1.8f, 0.2165146f);
        GameFlow.plateValue[GameFlow.plateNum] += foodValue;

        // 設回 y 是為了防止重複點擊已在盤子上的肉
        stillcooking = "y";
    }

    IEnumerator cookTimer()
    {
        yield return new WaitForSeconds(5);
        foodValue = 10;

        if (stillcooking == "y")
        {
            meat.material.color = new Color(.3f, .3f, .3f);
            stillcooking = "n"; // 煮好了，開放點擊

            // 如果你希望煮熟後煙就消失，取消下面這一行的註解：
            // if (smokeEffect != null) smokeEffect.Stop();
        }
    }
}