using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Button _restartButton;
    private TMP_Text _infoText;
    
    private void Start()
    {
        _restartButton = Utils.FindGameObjectByName("Restart Button").GetComponent<Button>();
        _restartButton.onClick.AddListener(RestartLevel);
        
        _infoText = Utils.FindGameObjectByName("Info Text (TMP)").GetComponent<TMP_Text>();
        _infoText.text = CarControl.Instance.MaxGearSpeedKMH();
    }
    
    private void RestartLevel()
    {
        SceneUtils.RestartScene();    
    }
}
