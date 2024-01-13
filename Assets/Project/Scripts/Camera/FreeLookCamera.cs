using Cinemachine;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private Transform carTransform;
    
    private void Start()
    {
        carTransform = CarControl.Instance.transform;
        
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        freeLookCamera.m_Follow = carTransform;
        freeLookCamera.m_LookAt = carTransform;
        freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
        freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        freeLookCamera.m_XAxis.m_InvertInput = false;
    }

}