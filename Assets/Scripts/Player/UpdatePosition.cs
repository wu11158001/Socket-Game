using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    private UpdatePosRequest updatePosRequest;
    private Transform gunTransform;

    private void Start()
    {
        updatePosRequest = GetComponent<UpdatePosRequest>();
        gunTransform = transform.Find("Weapon");

        InvokeRepeating("UpdatePosFun", 1, 1/30f);
    }

    /// <summary>
    /// 位置更新
    /// </summary>
    void UpdatePosFun()
    {
        Vector2 pos = transform.position;
        float characterRotZ = transform.eulerAngles.z;
        float gunRotZ = gunTransform.eulerAngles.z;

        updatePosRequest.SendRequest(pos, characterRotZ, gunRotZ);
    }
}
