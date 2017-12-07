using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ViewCheck
{
   
    public static bool IsInView(this Transform transform)
    {
        bool inView = false;
        if (transform == null)
        {
            return false;
        }
        
        foreach (var item in Camera.allCameras)
        {
            Vector3 screenPoint = item.WorldToViewportPoint(transform.position);
            inView = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }
        return inView;
    }

}

