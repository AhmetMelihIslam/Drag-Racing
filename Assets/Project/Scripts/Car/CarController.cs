using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : SingletonDestroy<CarControl>
{
    [SerializeField] private List<Wheel> wheels;
    private Rigidbody _carRb;
    
    private readonly float _maxAcceleration = 30.0f;
    private readonly float _brakeAcceleration = 60.0f;
    
    private MoveType _moveInput;
    
    // Gear variables
    [SerializeField] private int maxGears = 5;
    private bool _isGearUp;
    private bool _isGearDown;
    private int _currentGear = 1;
    private readonly float gearShiftDelay = 1.0f;
    private readonly float minRPMToShift = 1000.0f;
    private readonly float[] _maxSpeedGearKmh = { 52.89f, 89.85f, 127.20f, 174.13f, 241.69f };
    private readonly float[] _minRPM = { 0, 4687, 4980, 5108, 5255 };
    private readonly float[] _maxRPM = { 7200, 7069, 6794, 6456, 7200 };

    // Trail
    private TrailRenderer[] Tyremarks;
    private bool _isBrake;
    
    private void SetInput(MoveType input) => _moveInput = input;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        Tyremarks = transform.GetComponentsInChildren<TrailRenderer>();
    }
    
    private void Update()
    {
        AnimateWheels();
    }

    private void LateUpdate()
    {
        Gas();
        Brake();
        ShiftGears();
        CheckBrake();
    }

    private void Gas()
    {
        if (_isGearUp || _isGearDown) return;
        
        if (_moveInput == MoveType.Gas && CarKmh < CarMaxKMH)
        {
            SetWheelMotorTorque(600 * _maxAcceleration * Time.deltaTime);
        }
        else
        {
            SetWheelMotorTorque();
        }
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || _moveInput == MoveType.Brake)
        {
            SetWheelBrakeTorque(300 * _brakeAcceleration * Time.deltaTime);
            _isBrake = true;
        }
        else
        {
            SetWheelBrakeTorque();
            _isBrake = false;
        }
    }
    
    private void AnimateWheels()
    {
        // Rotation Amount = Vehicle Travel Distance / Wheel Circumference
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);

            // Get the wheel circumference
            float wheelCircumference = wheel.wheelCollider.radius * 2 * Mathf.PI;

            // Get the distance traveled by the vehicle
            float vehicleTravelDistance = Vector3.Distance(pos, wheel.wheelModel.transform.position);

            // Calculate the rotation amount
            float rotationAmount = vehicleTravelDistance / wheelCircumference;

            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot * Quaternion.Euler(0, rotationAmount * 360, 0);
        }
    }

    #region Whell Motor and Brake Torque Funcitons

    private void SetWheelMotorTorque(float var = 0)
    {
        foreach (var wheel in wheels)
        {
            wheel.SetMotorTorque(var);
        }
    }
    private void SetWheelBrakeTorque(float var = 0)
    {
        foreach (var wheel in wheels)
        {
            wheel.SetBrakeTorque(var);
        }
    }

    #endregion

    #region Trail start and stop functions

    private void CheckBrake()
    {
        if (_isBrake) StartEmmiter();
        else StopEmmiter();
    }
    private void StartEmmiter()
    {
        foreach (var T in Tyremarks)
        {
            T.emitting = true;
        }
    }
    private void StopEmmiter()
    {
        foreach (var T in Tyremarks)
        {
            T.emitting = false;
        }
    }

    #endregion
    
    #region Gear Functions
    
    private void ShiftGears()
    {
        float rpm = CalculateRPM();

        // Automatic gear shift
        if (rpm > minRPMToShift && _currentGear < maxGears && CarKmh > _maxSpeedGearKmh[_currentGear - 1])
        {
            _currentGear++;
            
            AdjustRPMForGearUp();
            CheckGearIncrease();
        }
        else if (_currentGear > 1 && CarKmh < _maxSpeedGearKmh[_currentGear - 2])
        {
            _currentGear--;
            StartCoroutine(nameof(GearDown));
        }
    }
    private void AdjustRPMForGearUp()
    {
        // After gear change RPM change
        float wheelRPM = wheels[0].wheelCollider.rpm;
        float finalDriveRatio = 3.42f;
        float engineRPM = wheelRPM * finalDriveRatio;

        // Decrease the engine speed by a certain percentage
        float rpmDecreasePercentage = 0.5f;

        engineRPM *= rpmDecreasePercentage;

        // Update the engine RPM
        SetWheelMotorTorque(engineRPM / 3.42f);

        StartCoroutine(nameof(GearUp));
    }

    private IEnumerator GearUp()
    {
        _isGearUp = true;
        yield return new WaitForSeconds(gearShiftDelay);
        _isGearUp = false;
    }
    private IEnumerator GearDown()
    {
        _isGearDown = true;
        yield return new WaitForSeconds(gearShiftDelay);
        _isGearDown = false;
    }

    private void CheckGearIncrease()
    {
        if (_currentGear < maxGears)
        {
            AdjustRPMForGearUp();
        }
    }

    #endregion
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance._isRaceFinish = true;
            _carRb.velocity = Vector3.zero;
            enabled = false;
        }
    }
    
    #region Button Functions

    public void GasEnter()
    {
        SetInput(MoveType.Gas);
    }
    
    public void GasExit()
    {
        SetInput(MoveType.NoGas);
    }
    
    public void BrakeEnter()
    {
        SetInput(MoveType.Brake);
    }
    
    public void BrakeExit()
    {
        SetInput(MoveType.NoBrake);
    }

    #endregion

    #region Getters Text Functions
        
    public string MaxGearSpeedKMH()
    {
        if (_maxSpeedGearKmh is null) InitMaxSpeedGearKMH();
        
        string gearText = "GEAR INFO\n";
        for (int i = 0; i < _maxSpeedGearKmh.Length; i++)
        {
            gearText += $"GEAR {i + 1} KM/H --> {_maxSpeedGearKmh[i]}\n";
        }

        return gearText;
    }
    
    public string GetSpeedMeterText() =>
        $"{Mathf.Round(CarKmh)}\nKM/H";
    
    public string GetRPMGearMeterText() =>
        $"{Mathf.Round(CalculateRPM())}";
    
    public string GetGearText() =>
        $"GEAR\n{_currentGear}";
        
    public float CalculateRPM()
    {
        // Assuming the wheels are spinning at a constant rate
        // Adjust this calculation based on your specific implementation
        float wheelRPM = wheels[0].wheelCollider.rpm;

        // Calculate the engine RPM based on the wheel RPM and final drive ratio.
        float finalDriveRatio = 3.42f;
        float engineRPM = wheelRPM * finalDriveRatio;

        return Mathf.Abs(engineRPM);
    }
    
    #endregion
    
    #region Getters
        
    public float CarVelocity => _carRb.velocity.magnitude;
    public float CarKmh => _carRb.velocity.magnitude * 3.6f;
    public float CarMaxKMH => 250f;

    #endregion
}
