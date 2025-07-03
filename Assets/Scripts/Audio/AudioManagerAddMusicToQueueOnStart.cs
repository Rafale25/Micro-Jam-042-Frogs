using UnityEngine;

public class AudioManagerAddMusicToQueueOnStart : MonoBehaviour
{
    [SerializeField] private string _musicName;
    [SerializeField] private bool _loop = false;

    void Start()
    {
        AudioManager.AddMusicToQueue(_musicName, _loop);
    }
}
