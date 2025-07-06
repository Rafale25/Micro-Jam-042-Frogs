using UnityEngine;
using UnityEngine.UI;

public class ShowCrownOnHighscore : MonoBehaviour
{
    [SerializeField] private Image _image;

    void Start()
    {
        var store = new HighscoreStoreWeb();
        float latestScore = store.GetLatestScore();
        float highScore = store.GetHighScore();

        if (highScore == latestScore)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
