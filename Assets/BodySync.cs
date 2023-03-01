using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyInfo
{
    public Transform point;
    public Transform joint;
}
public class BodySync : MonoBehaviour
{
    private void Awake()
    {
    }
    public BodyInfo[] bodyInfos;
    private void Update()
    {
        //for (int i = 0; i < bodyInfos.Length; i++)
        //{
        //    bodyInfos[i].joint.localPosition = bodyInfos[i].point.localPosition;
        //}
    }
}
