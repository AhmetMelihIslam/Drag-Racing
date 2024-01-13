using UnityEngine;

public class VehicleInstrumentPanel : MonoBehaviour
{
    [SerializeField] private InstrumentPanel _speedometer;
    [SerializeField] private InstrumentPanel _tachometer;
    
    private void Start()
    {
        _speedometer.SetArrow(Utils.FindGameObjectByName("SpeedMeterArrow").GetComponent<RectTransform>());
        _tachometer.SetArrow(Utils.FindGameObjectByName("RPMGearMeterArrow").GetComponent<RectTransform>());
    }

    private void LateUpdate()
    {
        _speedometer.SetAngles(CarControl.Instance.CarKmh);
        _tachometer.SetAngles(CarControl.Instance.CalculateRPM());
    }
}
