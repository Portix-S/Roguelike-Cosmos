using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] mobileButtons;
    DeviceType system;
    bool isMobileDevice;
    private void Start()
    {
        system = SystemInfo.deviceType;
        if(system == DeviceType.Desktop)
        {
            ChangeStateMobileButtons(false);
        }
        isMobileDevice = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMobileDevice = !isMobileDevice;
            ChangeStateMobileButtons(isMobileDevice);
            if (isMobileDevice)
                system = DeviceType.Handheld;
            else
                system = DeviceType.Desktop;
        }
    }

    void ChangeStateMobileButtons(bool state)
    {
        for (int i = 0; i < mobileButtons.Length; i++)
        {
            mobileButtons[i].SetActive(state);
        }
    }

    public void AttackButton()
    {
        Debug.Log("Attack");
    }

    public void DashButton()
    {
        RbPlayerMovement playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<RbPlayerMovement>();
        playerMov.StartCoroutine("Dash");
    }
}
