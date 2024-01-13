using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    private float _elapsedTime;
    private bool _isTimerRunning;

    private void Start()
    {        
        _timerText = Utils.FindGameObjectByName("TIMER Text (TMP)").GetComponent<TMP_Text>();

        _elapsedTime = 0f;
        _isTimerRunning = false;
        UpdateTimerText();
    }

    private void Update()
    {
        if (GameManager.Instance._gameStart && !GameManager.Instance._isRaceFinish)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
        
        if (_isTimerRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        int fraction = Mathf.FloorToInt((_elapsedTime * 100f) % 100f);

        _timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
    }

    private void StartTimer()
    {
        _isTimerRunning = true;
    }

    private void StopTimer()
    {
        _isTimerRunning = false;
    }
}