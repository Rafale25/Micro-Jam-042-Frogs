using TMPro;
using UnityEngine;

public class Chrono : MonoBehaviour
{
    [SerializeField] private float chronoTimer = 0;
    // [SerializeField] private int chronoDisplay = 0;

    [SerializeField] private bool timerRunning = false;
    [SerializeField] private TMP_Text timerText;

    void Update()
    {
        if (timerRunning)
        {
            chronoTimer += Time.deltaTime;
            timerText.SetText(chronoTimer.ToString("F1"));
        }
    }

    public void StartChrono()
    {
        timerRunning = true;
    }

    public void StopChrono()
    {
        timerRunning = false;

        // float currentHighscore = PlayerPrefs.GetFloat("Highscore");
        float currentHighscore = HighscoreStore.Highscore <= 1f ? 100000f : HighscoreStore.Highscore;

        // PlayerPrefs.SetFloat("LastScore", chronoTimer);
        HighscoreStore.LastScore = chronoTimer;

        if (chronoTimer < currentHighscore)
        {
            HighscoreStore.MadeNewHighscore = true;
            HighscoreStore.Highscore = chronoTimer;

            PlayerPrefs.SetFloat("Highscore", chronoTimer);
        }
        else
        {
            HighscoreStore.MadeNewHighscore = false;
        }
    }
}
