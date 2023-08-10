using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoomNew : MonoBehaviour
{
    Vector3 touchStart;
    //public GameObject skillTreeUI;
    //public RectTransform rt;
    public float resetTransformAmount = 50f;
    float minZoom = 0.5f;
    float maxZoom = 2f;
    [SerializeField] float panSpeed = 5f;
    [SerializeField] CinemachineVirtualCamera cineVc;
    CinemachineConfiner confiner;
    Transform camera;
    public bool cameraTransitioningIn;
    // Update is called once per frame

    private void Start()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            panSpeed *= 2f;
        }
        camera = Camera.main.transform;
    }

    // Maybe when zoomingOut, after certain amount, reset position to 0? or maybe slowly change it back?
    void Update()
    {
        Debug.Log("Normal " + camera.position + " " + transform.position);
        Debug.Log("Local " + camera.localPosition + transform.localPosition);
        if (camera.localPosition == transform.position && cameraTransitioningIn)
            cameraTransitioningIn = false;
        //*

        if (camera.localPosition != transform.position && !cameraTransitioningIn)
            transform.position = camera.localPosition; // Tentar deixar suave
        //*/
        // InitialTouch
        if(Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        // If zooming with touch
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(-difference * 0.005f); // 0.01f sensibilidade é um bom valor

        } // Checks if Dragging input on screen
        else if (Input.GetMouseButton(0))
        {
            if (!Input.GetMouseButtonDown(0))
            {
                Vector3 direction = touchStart - Input.mousePosition;
                transform.localPosition += direction * panSpeed * Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -904f);
                touchStart -= direction / 10f; // Slows down movement when mouse is stationary
            }
        }
        // Checks if zoomming with mouse
        if(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > Mathf.Epsilon)
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        // Checks if zoomingOut
        increment = Mathf.Clamp(cineVc.m_Lens.OrthographicSize - increment, minZoom, maxZoom);
        cineVc.m_Lens.OrthographicSize = increment;
        if(cineVc.m_Lens.OrthographicSize > maxZoom*0.9f)
        {
            transform.localPosition = Vector3.zero;
        }
    }
    
    /*
    public void CheckTransform()
    {
        float amount = resetTransformAmount + (0.3f * rt.anchoredPosition.x);
        float negAmount = resetTransformAmount - (0.3f * rt.anchoredPosition.x);

        // Getting image back to center of screen so it doesn't break the boundaries
        if (rt.anchoredPosition.x > amount)
            rt.anchoredPosition -= new Vector2(amount, 0f);
        if (rt.anchoredPosition.x < -negAmount)
            rt.anchoredPosition += new Vector2(negAmount, 0f);
        else if(rt.anchoredPosition.x < amount)
            rt.anchoredPosition = new Vector2(0f, rt.anchoredPosition.y);

        if (rt.anchoredPosition.y > resetTransformAmount + (0.2f * rt.anchoredPosition.y))
            rt.anchoredPosition -= new Vector2(0f, resetTransformAmount + (0.2f * rt.anchoredPosition.y));
        else if (rt.anchoredPosition.y < -resetTransformAmount - (0.2f * rt.anchoredPosition.y))
            rt.anchoredPosition += new Vector2(0f, resetTransformAmount + (-0.2f * rt.anchoredPosition.y));
        else
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 0f);
    }
    //*/
}
