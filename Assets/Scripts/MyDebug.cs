using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    public static void DrawCircle(Vector3 position, float radius, Color color, float time, int nStep)
    {
        float angleStep = 360 / nStep;
        Quaternion rotateQuat;
        Quaternion rotateQuat2;
        Vector3 firstPos;
        Vector3 secondPos;

        for (int i = 0; i <= nStep; i++)
        {
            rotateQuat = Quaternion.AngleAxis(angleStep*i, Vector3.forward);
            rotateQuat2 = Quaternion.AngleAxis(angleStep*(i+1), Vector3.forward);
            firstPos = position + rotateQuat * Vector3.left * radius;
            secondPos = position + rotateQuat2 * Vector3.left * radius;
            Debug.DrawLine(firstPos, secondPos, color, time);
        }

    }
}
