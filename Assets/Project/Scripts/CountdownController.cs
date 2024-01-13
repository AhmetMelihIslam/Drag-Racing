using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CountdownController : SingletonDestroy<CountdownController>
{
    private AudioSource _audioSrc;

    private AudioClip _countDown;
    private AudioClip _startSound;
    
    private TMP_Text _countDownText;

    [SerializeField] private int countdownTime;
    

    private void Start()
    {
        GameManager.Instance._gameStart = false;
        
        _audioSrc = GetComponent<AudioSource>();
        _countDownText = Utils.FindGameObjectByName("Countdown Text (TMP)").GetComponent<TMP_Text>();
        _countDown = Resources.Load<AudioClip>("Sounds/countdown/countdown-a");
        _startSound = Resources.Load<AudioClip>("Sounds/start/qubodup-(Ulrich Metzner Bell)-start_race");
        
        StartCoroutine(CountdownToStart());
    }

    private IEnumerator CountdownToStart()
    {
        yield return new WaitForSeconds(1.5f);
        
        while(countdownTime > 0)
        {
            // Count down text update
            CountDownTextUpdate();
            _audioSrc.PlayOneShot(_countDown);

            yield return new WaitForSeconds(1f);

            countdownTime--;
            // count down after change text again
            CountDownTextUpdate();
        }

        _audioSrc.PlayOneShot(_startSound);
        GameManager.Instance._gameStart = true;
        _countDownText.text = "START";
        yield return new WaitForSeconds(1f);

        _countDownText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void CountDownTextUpdate() =>
        _countDownText.text = $"{countdownTime}";
}
