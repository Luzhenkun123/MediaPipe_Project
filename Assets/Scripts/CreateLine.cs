using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    public List<Vector2Int> pointList = new List<Vector2Int>();
    List<LineCode> lineCodes = new List<LineCode>();
    [ContextMenu("添加节点")]
    public void AddPoints()
    {
        if(lineCodes!=null&&lineCodes.Count>0)
        {
            for (int i = 0; i < lineCodes.Count; i++)
            {
                if(lineCodes[i]!=null)
                    Destroy(lineCodes[i].gameObject);
            }
        }
        lineCodes.Clear();

        for (int i = 0; i < pointList.Count; i++)
        {
            GameObject line = new GameObject(pointList[i].x + "_" + pointList[i].y);
            line.transform.SetParent(transform);
            line.AddComponent<LineRenderer>();
            lineCodes.Add(line.AddComponent<LineCode>());
        }
        Transform parent = transform.parent.Find("BodyPoint");
        for (int i = 0; i < lineCodes.Count; i++)
        {
            lineCodes[i].origin = parent.GetChild(pointList[i].x);
            lineCodes[i].destination = parent.GetChild(pointList[i].y);
        }
    }
}
