using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public GameObject test;
    float minZoom = 0.725f;
    float maxZoom = 3.3f;
    // Update is called once per frame

    private void Start()
    {
        test.SetActive(true);
    }


    // Maybe when zoomingOut, after certain amount, reset position to 0? or maybe slowly change it back?
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f); // 0.01f sensibilidade é um bom valor

        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            test.transform.position -= direction * 5f;
            test.transform.position = new Vector3(test.transform.position.x, test.transform.position.y, 0f);
            touchStart -= direction/10f;
        }
        if(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > Mathf.Epsilon)
            Zoom(-Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        increment = Mathf.Clamp(test.transform.localScale.x - increment, minZoom, maxZoom);
        test.transform.localScale = new Vector3(increment, increment, 0f);
        
    }
}
