using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    private CarControl _carControl;
    private TMP_Text _speedMeter;
    private TMP_Text _rpmGearMeter;
    
    private Button _gasButton;
    private Button _brakeButton;
    
    private Button _mouseLockButton;
    private TMP_Text _mouseLockButtonText;
    
    private TMP_Text _raceFinishText;
    
    private TMP_Text _gearText;

    private void Start()
    {
        _speedMeter = Utils.FindGameObjectByName("SpeedMeter Text (TMP)").GetComponent<TMP_Text>();
        _rpmGearMeter = Utils.FindGameObjectByName("RPMGearMeter Text (TMP)").GetComponent<TMP_Text>();
        
        _gasButton = Utils.FindGameObjectByName("Gas Button").GetComponent<Button>();
        _gasButton.interactable = false;
        _brakeButton = Utils.FindGameObjectByName("Break Button").GetComponent<Button>();
        _brakeButton.interactable = false;
        
        _mouseLockButton = Utils.FindGameObjectByName("Mouse Lock Button").GetComponent<Button>();
        _mouseLockButton.onClick.AddListener(MouseLockButton);
        _mouseLockButtonText = _mouseLockButton.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        
        _raceFinishText = Utils.FindGameObjectByName("RACE FINISH Text (TMP)").GetComponent<TMP_Text>();
        RaceFinishTextActive(false);
        
        _gearText = Utils.FindGameObjectByName("Gear Text (TMP)").GetComponent<TMP_Text>();
        
        _carControl = CarControl.Instance;
        
        CursorUtils.SetCursorConfined();
    }

    private void Update()
    {
        if (GameManager.Instance._isRaceFinish)
        {
            RaceFinishTextActive(true);
            _gasButton.interactable = false;
            _brakeButton.interactable = false;
            _speedMeter.text = _carControl.GetSpeedMeterText();
            _rpmGearMeter.text = _carControl.GetRPMGearMeterText();
            _gearText.text = _carControl.GetGearText();
            CursorUpdate();
            return;
        }
        
        CursorUpdate();
        
        if (GameManager.Instance._gameStart)
        {
            ButtonActive();
        }
        
        _speedMeter.text = _carControl.GetSpeedMeterText();
        _rpmGearMeter.text = _carControl.GetRPMGearMeterText();
        _gearText.text = _carControl.GetGearText();
    }

    private void CursorUpdate()
    {
        if (CursorUtils.IsCursorLocked() && Input.GetKey(KeyCode.Escape))
        {
            MouseLockButton();
        }
    }

    private void MouseLockButton()
    {
        if (CursorUtils.IsCursorConfined() || CursorUtils.IsCursorNone())
        {
            _mouseLockButtonText.text = "MOUSE" +
                                        "\nCONFINED" +
                                        "\nPress" +
                                        "\nESC";
            CursorUtils.SetCursorLocked();
        }
        else if (CursorUtils.IsCursorLocked())
        {
            _mouseLockButtonText.text = "MOUSE\nLOCK";
            CursorUtils.SetCursorConfined();
        }
    }

    private void ButtonActive()
    {
        if (_gasButton.interactable || _brakeButton.interactable) return;
        
        _gasButton.interactable = true;
        _brakeButton.interactable = true;
    }

    private void RaceFinishTextActive(bool var)
    {
        _raceFinishText.gameObject.SetActive(var);
    }

    #region Gas and Brake Event Trigger Functions (Point Down, Point Up)
    
    public void GasEnter()
    {
        if (_gasButton.interactable) _carControl.GasEnter();
    }
    
    public void GasExit()
    {
        if (_gasButton.interactable) _carControl.GasExit();
    }

    public void BrakeEnter()
    {
        if (_brakeButton.interactable) _carControl.BrakeEnter();
    }

    public void BrakeExit()
    {
        if (_brakeButton.interactable) _carControl.BrakeExit();
    }
    
    #endregion
}
