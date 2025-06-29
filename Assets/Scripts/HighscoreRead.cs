using TMPro;
using UnityEngine;

public class HighscoreRead : MonoBehaviour
{
    private TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        float score = PlayerPrefs.GetFloat("Highscore");
        _text.text = score.ToString("F2");
    }
}
