using TMPro;
using UnityEngine;

public class HighscoreRead : MonoBehaviour
{
    private TextMeshProUGUI _text;

    [SerializeField] private bool latestScore = false;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        var store = new HighscoreStoreWeb();

        float score = latestScore ? store.GetLatestScore() : store.GetHighScore();
        _text.text = score.ToString("F2");
    }
}
