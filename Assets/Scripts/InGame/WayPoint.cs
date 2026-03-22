using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public bool isStopPoint = false; // 적이 멈춰야 하는 지점인지 여부

    void Start()
    {
        //하위 오브젝트 SetAcive(false)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
