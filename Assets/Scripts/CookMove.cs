using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMove : MonoBehaviour
{
    private int foodValue = 0;
    private MeshRenderer meat;
    private string stillcooking = "y"; // y=正在煮, n=熟了可拿, done=已在盤子上

    [Header("特效")]
    [SerializeField] private ParticleSystem smokeEffect;

    [Header("音效")]
    [SerializeField] private AudioSource grillAudio;
    [SerializeField] private AudioSource placeAudio;

    void Start()
    {
        meat = GetComponent<MeshRenderer>();

        if (smokeEffect != null)
        {
            smokeEffect.gameObject.SetActive(true);
            smokeEffect.Play();
        }

        if (grillAudio != null)
        {
            grillAudio.loop = true;
            grillAudio.Play();
        }

        StartCoroutine(cookTimer());
    }

    public void Interact()
    {
        // ⭐【關鍵修改】
        // 原本是 if (stillcooking == "y") return;
        // 改成下面這樣：只要不是 "n" (代表正在煮"y" 或是 已經拿過了"done")，就直接擋掉
        if (stillcooking != "n") { return; }

        // --- 移到盤子上的動作 ---

        if (placeAudio != null)
        {
            placeAudio.PlayOneShot(placeAudio.clip);
        }

        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }

        if (grillAudio != null)
        {
            grillAudio.Stop();
        }

        transform.position = new Vector3(GameFlow.plateXpos, 1.8f, 0.2165146f);
        GameFlow.plateValue[GameFlow.plateNum] += foodValue;

        // 設定為 "done" 代表已經結束，搭配上面的判斷，之後再點就不會有反應了
        stillcooking = "done";

        // (選用) 為了保險起見，也可以在這裡直接銷毀碰撞體，讓滑鼠物理上點不到
        // Destroy(GetComponent<Collider>());
    }

    IEnumerator cookTimer()
    {
        yield return new WaitForSeconds(5);
        foodValue = 10;

        // 只有在還沒被移走的情況下才變色
        if (stillcooking == "y")
        {
            meat.material.color = new Color(.3f, .3f, .3f);
            stillcooking = "n"; // 設定為 "n"，這時候 Interact 才能通過檢查
            Debug.Log("肉熟了！可以拿取");
        }
    }
}