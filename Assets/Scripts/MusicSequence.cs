using UnityEngine;

public class MusicSequence : MonoBehaviour
{
    [SerializeField] private bool _loopLast;
    [SerializeField] private string[] _musicsNames;

    private int _musicIndex = 0;

    void Start()
    {
        AudioManager.SetMusicLoop(false);
        PlayNext();
    }

    void Update()
    {
        if (_musicIndex >= _musicsNames.Length) return;
        if (!AudioManager.IsMusicPlaying())
        {
            PlayNext();
        }
    }

    private void PlayNext()
    {
        if (_musicIndex == _musicsNames.Length - 1)
        {
            AudioManager.SetMusicLoop(_loopLast);
        }

        AudioManager.PlayMusic(_musicsNames[_musicIndex]);
        _musicIndex += 1;
    }
}
