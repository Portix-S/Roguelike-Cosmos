using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    CinemachineVirtualCamera virtualCamera;
    float shakerTimer = 0f;
    float shakerTimerTotal = 0f;
    float startingIntensity = 0f;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        if (SystemInfo.deviceType == DeviceType.Handheld)
            startingIntensity = 2 * intensity;
        else
            startingIntensity = intensity;
        shakerTimerTotal = time;
        shakerTimer = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakerTimer > 0f)
        {
            shakerTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1f - shakerTimer / shakerTimerTotal);
        }
    }
}
