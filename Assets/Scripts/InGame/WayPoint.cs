using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    void Start()
    {
        //하위 오브젝트 SetAcive(false)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
