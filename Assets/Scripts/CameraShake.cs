using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    #region Singleton
    private static CameraShake _instance;
    public static CameraShake Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CameraShake>();
            return _instance;
        }
    }
    #endregion

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;


    private void Start()
    {
        cinemachineVirtualCamera.m_Follow = Player.Instance.transform;
    }

    public void ShakeCamera(float intensiity, float frequency, float time)
    {
        // getting the cinemachine perline noise
        CinemachineBasicMultiChannelPerlin perline =
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perline.m_AmplitudeGain = intensiity;
        perline.m_FrequencyGain = frequency;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // timer countdown
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                // time over
                CinemachineBasicMultiChannelPerlin perline =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                perline.m_AmplitudeGain = 0f;
            }
        }
    }
}
