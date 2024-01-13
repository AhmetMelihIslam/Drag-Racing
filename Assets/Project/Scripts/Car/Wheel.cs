using UnityEngine;

[System.Serializable]
public struct Wheel
{
    public GameObject wheelModel;
    public WheelCollider wheelCollider;

    public void SetMotorTorque(float var = 0)
    {
        wheelCollider.motorTorque = var;
    }
    public void SetBrakeTorque(float var = 0)
    {
        wheelCollider.brakeTorque = var;
    }
}