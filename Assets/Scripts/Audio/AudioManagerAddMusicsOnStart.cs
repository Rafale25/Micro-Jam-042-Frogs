using UnityEngine;

public class AddMusicToQueueAtStart : MonoBehaviour
{
    [System.Serializable] private class MusicInfo { public string name; public bool loop; }

    [SerializeField] private bool _clearQueue = true;
    [SerializeField] private bool _stopCurrentMusic = true;
    [SerializeField] private MusicInfo[] _musics;

    void Start()
    {
        if (_clearQueue)
        {
            AudioManager.ClearMusicQueue();
        }

        foreach (var musicInfo in _musics)
        {
            AudioManager.AddMusicToQueue(musicInfo.name, musicInfo.loop);
        }

        if (_stopCurrentMusic)
        {
            AudioManager.StopMusic();
        }
    }
}
