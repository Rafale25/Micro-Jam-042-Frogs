using UnityEngine;

public class AddMusicToQueueAtStart : MonoBehaviour
{
    [System.Serializable] class MusicInfo { public string name; public bool loop; }
    [SerializeField] private MusicInfo[] _musics;

    void Start()
    {
        foreach (var musicInfo in _musics) {
            AudioManager.AddMusicToQueue(musicInfo.name, musicInfo.loop);
        }
    }
}
