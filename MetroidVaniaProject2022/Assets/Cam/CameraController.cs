using System.Collections;
using UnityEngine;
//import cinemachine
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    private float shakeTimer;

    [SerializeField] private CinemachineBasicMultiChannelPerlin vmNoise;

    public void ShakeCamera(float intensity, float time)
    {
        vmNoise.m_AmplitudeGain = intensity;
        shakeTimer = time;
        
    }

    private void Update()
    {
        if (shakeTimer >= 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                vmNoise.m_AmplitudeGain = 0f;
            }
        }
    }

    private void Start()
    {
        //vcam = GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        vmNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

       

    }



}