using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CurveCalculator : MonoBehaviour
{

    public static CurveCalculator instance;

    [SerializeField] float maxDeviation;

    GameObject start, mid = null;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 CalcCurveMiddle(Transform startPos, Transform endPos, string side)
    {
        Vector3 midllePos = (startPos.position + endPos.position) / 2;

        if (start == null)
            start = new GameObject(startPos.name);

        if (mid == null)
            mid = new GameObject("mid");

        start.transform.position = startPos.position;
        start.transform.LookAt(endPos.position);

        start.transform.rotation = Quaternion.Euler(start.transform.rotation.eulerAngles.x, -90, start.transform.rotation.eulerAngles.z);


        float dis = Vector3.Distance(startPos.position, endPos.position);

        if (side == "Up")
            midllePos += start.transform.up * (dis / maxDeviation);
        else if (side == "Down")
            midllePos += -start.transform.up * (dis / maxDeviation);

        mid.transform.position = midllePos;


        return midllePos;
    }

    private static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }
    public Vector3 CurveMove(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Lerp(a, b, t);
        Vector3 p1 = Lerp(b, c, t);
        return Lerp(p0, p1, t);
    }
}