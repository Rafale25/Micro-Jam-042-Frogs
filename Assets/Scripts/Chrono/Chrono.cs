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
        }
        timerText.SetText(chronoTimer.ToString("F1"));
    }

    public void StartChrono()
    {
        timerRunning = true;
    }

    public void StopChrono()
    {
        timerRunning = false;
    }

    public void ResetChrono()
    {
        chronoTimer = 0f;
    }

    public void SaveHighscore()
    {
        var store = new HighscoreStoreWeb();
        float highScore = store.GetHighScore();

        float currentHighscore = highScore <= 1f ? 100000f : highScore;

        if (chronoTimer < currentHighscore)
        {
            store.SetHighScore(chronoTimer);
        }

        store.SetLatestScore(chronoTimer);
    }
}
