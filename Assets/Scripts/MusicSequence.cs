using UnityEngine;

public class MusicSequence : MonoBehaviour
{
    [SerializeField] private bool _loopLast;
    [SerializeField] private string[] _musicsNames;

    private int _musicIndex = 0;

    void Update()
    {
        if (_musicIndex >= _musicsNames.Length) return;
        if (!AudioManager.IsMusicPlaying())
        {
            if (_musicIndex == _musicsNames.Length - 1)
            {
                AudioManager.SetMusicLoop(_loopLast);
            }

            AudioManager.PlayMusic(_musicsNames[_musicIndex]);
            _musicIndex += 1;
        }
    }
}
