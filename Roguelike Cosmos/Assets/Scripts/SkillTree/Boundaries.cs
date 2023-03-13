using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public GameObject[] boundariesObj;
    float topBound;
    float botBound;
    float leftBound;
    float rightBound;
    Vector3 objPos;
    Vector3 lastPos;
    public RectTransform rt;
    bool outsideLimitsHorizontal;
    bool outsideLimitsVertical;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        objPos = rt.anchoredPosition;
        topBound = Camera.main.ScreenToViewportPoint(boundariesObj[0].transform.position).y; // >1
        rightBound = Camera.main.ScreenToViewportPoint(boundariesObj[1].transform.position).x; // >1
        botBound = Camera.main.ScreenToViewportPoint(boundariesObj[2].transform.position).y; // <0
        leftBound = Camera.main.ScreenToViewportPoint(boundariesObj[3].transform.position).x; // <0
        if (topBound < 1 || botBound > 0)
        {
            outsideLimitsVertical = true;
            objPos.y = Mathf.Clamp(objPos.y, lastPos.y, lastPos.y);
            rt.anchoredPosition = objPos;
        }
        else
            outsideLimitsVertical = false;

        if (leftBound > 0 || rightBound < 1)
        {
            outsideLimitsHorizontal = true;
            objPos.x = Mathf.Clamp(objPos.x, lastPos.x, lastPos.x);
            rt.anchoredPosition = objPos;
        }
        else
            outsideLimitsHorizontal = false;


        if (!outsideLimitsHorizontal)
        {
            lastPos.x = rt.anchoredPosition.x;
        }
        if (!outsideLimitsVertical)
        {
            lastPos.y = rt.anchoredPosition.y;
        }
    }
}
