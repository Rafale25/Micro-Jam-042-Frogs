using UnityEngine;

public class MusicSequence : MonoBehaviour
{
    // [SerializeField] private AudioClip[] _musics;
    [SerializeField] private string[] _musicsNames;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _loopLast;

    private int _musicIndex = 0;

    void Update()
    {
        if (_musicIndex >= _musicsNames.Length) return;
        if (!_audioSource.isPlaying)
        {
            if (_musicIndex == _musicsNames.Length - 1) {
                _audioSource.loop = _loopLast;
            }

            AudioManager.PlayMusic(_musicsNames[_musicIndex]);
            _musicIndex += 1;
        }
    }
}
