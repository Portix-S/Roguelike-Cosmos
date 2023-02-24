using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Vector2 boundaries;
    public GameObject[] boundariesObj;
    public float topBound;
    public float botBound;
    public float leftBound;
    public float rightBound;
    public Vector3 objPos;
    public Vector3 lastPos;
    public RectTransform rt;
    public bool outsideLimitsHorizontal;
    public bool outsideLimitsVertical;

    // Start is called before the first frame update
    void Start()
    {
        boundaries = new Vector2(Screen.width/2, Screen.height/2);
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
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

        //objPos.x = Mathf.Clamp(objPos.x, -rt.sizeDelta.x / 2, rt.sizeDelta.x / 2);
        //objPos.x = Mathf.Clamp(objPos.x, boundaries.x * - 1, boundaries.x);
        //objPos.y = Mathf.Clamp(objPos.y, -boundaries.y, boundaries.y);
        //rt.anchoredPosition = objPos;
        //transform.localPosition = objPos;
    }
}
