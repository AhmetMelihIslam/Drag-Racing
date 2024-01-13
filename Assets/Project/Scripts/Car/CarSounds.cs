using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSounds : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private float _currentSpeed;

    private AudioSource _carAudio;
    private CarControl _carControl;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float _pitchFromCar;

    private void Start()
    {
        _carAudio = GetComponent<AudioSource>();
        _carControl = CarControl.Instance;
    }

    private void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        _currentSpeed = _carControl.CarVelocity;
        _pitchFromCar = _carControl.CarVelocity / 60f;

        if(_currentSpeed < minSpeed)
        {
            _carAudio.pitch = minPitch;
        }

        if(_currentSpeed > minSpeed && _currentSpeed < maxSpeed)
        {
            _carAudio.pitch = minPitch + _pitchFromCar;
        }

        if(_currentSpeed > maxSpeed)
        {
            _carAudio.pitch = maxPitch;
        }
    }
}