using UnityEngine;

public interface IHighScoreStore
{
    public void SetHighScore(float score);
    public void SetLatestScore(float score);
    public float GetHighScore();
    public float GetLatestScore();
}

public class HighscoreStoreWeb : IHighScoreStore
{
    const string KeyNameHighscore = "Highscore";
    const string KeyNameLatestHighscore = "PreviousHighscore";

    public void SetHighScore(float score)
    {
        // move highscore to previoushighscore
        // string highscoreStr = WebLocalStorage.LocalStorageGet(KeyNameHighscore);
        // WebLocalStorage.LocalStorageSet(KeyNameLatestHighscore, highscoreStr);

        // set new highscore
        WebLocalStorage.LocalStorageSet(KeyNameHighscore, score.ToString());
    }

    public void SetLatestScore(float score)
    {
        WebLocalStorage.LocalStorageSet(KeyNameLatestHighscore, score.ToString());
    }

    private float GetScore(string keyName)
    {
        string scoreStr = WebLocalStorage.LocalStorageGet(keyName);
        Debug.Log($"scoreStr: {scoreStr}");

        if (float.TryParse(scoreStr, out float score))
        {
            return score;
        }
        else
        {
            return 0f;
        }
    }

    public float GetHighScore() => GetScore(KeyNameHighscore);
    public float GetLatestScore() => GetScore(KeyNameLatestHighscore);
}
