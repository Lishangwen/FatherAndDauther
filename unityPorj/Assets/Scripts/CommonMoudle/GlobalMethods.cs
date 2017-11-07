using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMethods 
{
    // 获取两物体的距离
    static public float GetDistance(GameObject a,GameObject b)
    {
        float distance=0;
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;
        distance = Vector3.Distance(aPosition, bPosition);
        return distance;
    }
}
