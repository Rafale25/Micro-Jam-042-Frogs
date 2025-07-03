using UnityEngine;
using UnityEngine.UI;

public class ShowCrownOnHighscore : MonoBehaviour
{
    [SerializeField] private Image _image;

    void Start()
    {
        if (HighscoreStore.MadeNewHighscore)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }

        HighscoreStore.MadeNewHighscore = false;
    }
}
