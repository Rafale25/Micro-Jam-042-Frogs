using UnityEngine;

public class AudioManagerPlayOnStart : MonoBehaviour
{
    [SerializeField] private string _musicName;

    void Start()
    {
        AudioManager.PlayMusic(_musicName);
    }
}
