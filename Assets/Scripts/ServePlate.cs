using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServePlate : MonoBehaviour
{
    public int thisPlate = 0;
    void Start()
    {

    }
    void Update()
    {

    }
    public void Interact()
    {
        if (GameFlow.plateNum != thisPlate)
        {
            return; // 忽略這次點擊
        }
        bool isRight = (GameFlow.orderValue[thisPlate] == GameFlow.plateValue[thisPlate]);
        /*if (GameFlow.orderValue[thisPlate] == GameFlow.plateValue[thisPlate])
        {
            Debug.Log("correct! Time: " + " " + GameFlow.orderTimer[thisPlate]);
        }
        else
        {
            Debug.Log("Wrong! You made: " + GameFlow.plateValue[thisPlate]);
        }*/

        float timeLeft = GameFlow.orderTimer[thisPlate];
        StartCoroutine(ProcessServing(isRight, timeLeft));
        //GameFlow.emptyPlateNow = transform.position.x;
        //StartCoroutine(platereset(isRight));

    }
    /*IEnumerator platereset(bool isCorrect)
    {
        yield return new WaitForSeconds(.2f);
        GameFlow.emptyPlateNow = -1;
        if (isCorrect) // 只有是對的時候才加錢
        {
            GameFlow.totalCash += GameFlow.orderTimer[thisPlate] * .10f;
        }
        else
        {
            GameFlow.totalCash -= GameFlow.orderTimer[thisPlate] * .10f;
        }
        GameFlow.plateValue[thisPlate] = 0;
    }*/
    IEnumerator ProcessServing(bool isRight, float timeLeft)
    {
        GameFlow.emptyPlateNow = transform.position.x;
        yield return new WaitForSeconds(0.2f);
        GameFlow.emptyPlateNow = -3;


        if (isRight)
        {
            GameFlow.totalCash += timeLeft * 10;

            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);
        }
        else
        {
            GameFlow.totalCash -= 50;
            if (GameFlow.seatMap[thisPlate] != null)
                GameFlow.seatMap[thisPlate].Leave(true);
            Debug.Log("做錯了！");
        }


        GameFlow.plateValue[thisPlate] = 0;
    }
}
