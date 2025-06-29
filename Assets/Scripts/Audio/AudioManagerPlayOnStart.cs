using UnityEngine;

public class AudioManagerPlayOnStart : MonoBehaviour
{
    [SerializeField] private string _musicName;
    [SerializeField] private bool _loop;

    void Start()
    {
        AudioManager.SetMusicLoop(_loop);
        AudioManager.PlayMusic(_musicName);
    }
}
