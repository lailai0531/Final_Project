using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow instance;

    //public static int[] orderValue = { 111111, 111101, 011110 };
    public static int[] orderValue = { 0, 0, 0 };
    public static int[] plateValue = { 0, 0, 0 };
    public static float[] orderTimer = { 0, 0, 0 };

    public static Customer[] seatMap = new Customer[3];

    public static int plateNum = 0;
    public static float plateXpos = -0.678f;

    public Transform plateSelector;

    //public MeshRenderer[] currentPic;

    //public Texture[] orderPics;

    public static float emptyPlateNow = -1;

    public static float totalCash = 0;

    public Sprite[] orderPics;

    /*void Start()
    {
        for (int rep = 0; rep < 3; rep++)
        {
            if (orderValue[rep] == 110001)
                currentPic[rep].GetComponent<MeshRenderer>().material.mainTexture = orderPics[0];
            if (orderValue[rep] == 111111)
                currentPic[rep].GetComponent<MeshRenderer>().material.mainTexture = orderPics[1];
            if (orderValue[rep] == 111101)
                currentPic[rep].GetComponent<MeshRenderer>().material.mainTexture = orderPics[2];
            if (orderValue[rep] == 011110)
                currentPic[rep].GetComponent<MeshRenderer>().material.mainTexture = orderPics[3];
        }
    } */

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            plateNum += 1;
            plateXpos += 2.55f;

            if (plateNum > 2)
            {
                plateNum = 0;
                plateXpos = -0.75f;
            }
        }

        //orderTimer[0] -= Time.deltaTime;
        //orderTimer[1] -= Time.deltaTime;
        ///rderTimer[2] -= Time.deltaTime;

        plateSelector.transform.position = new Vector3(plateXpos, 1.02f, 0.254f);
    }
}
