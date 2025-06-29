using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public static AudioManager Instance;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _audioMixerGroupMusic;
    [SerializeField] private AudioMixerGroup _audioMixerGroupEffects;
    [SerializeField] private AudioMixerGroup _audioMixerGroupGameplay;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectsSource;

    [SerializeField] private Sound[] _sounds;
    private IDictionary<string, Sound> _soundsMap = new Dictionary<string, Sound>();

    private AudioPool _audioPool = new();

    [SerializeField] private int _poolSize;
    [SerializeField] private int _playingSoundsCount;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] // For when domain reloading is disabled
    static void Init()
    {
        if (Instance) SceneManager.activeSceneChanged -= Instance.ChangedActiveScene;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += ChangedActiveScene;

        foreach (Sound s in _sounds)
        {
            _soundsMap.Add(s.name, s);
        }
    }

    void Update()
    {
        _audioPool.UpdatePool();

        _poolSize = _audioPool.PoolCount;
        _playingSoundsCount = _audioPool.PlayingCount;
    }

    public static void PlayMusic(string name)
    {
        Instance._musicSource.clip = Instance._soundsMap[name].clip;
        Instance._musicSource.Play();
    }

    public static void StopMusic()
    {
        Instance._musicSource.Stop();
    }

    // PlayClip
    public static void PlaySound2D(string name, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!Instance._soundsMap.ContainsKey(name))
        {
            Debug.LogError("Sound " + name + " doesn't exist!");
            return;
        }

        Sound s = Instance._soundsMap[name];

        GameObject obj = Instance._audioPool.GetAvailablePoolObject();
        if (obj == null) return;
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        // obj.transform.SetParent(null, false);

        audioSource.clip = s.clip;
        audioSource.outputAudioMixerGroup = Instance._audioMixerGroupEffects; // effect audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0f;

        audioSource.Play();
    }

    // PlayClipAtPoint
    public static AudioSource PlaySound(
        Vector3 position,
        AudioClip clip,
        float volume = 1.0f,
        float pitch = 1.0f,
        float spatialBlend = 1.0f,
        float maxDistance = 20.0f)
    {
        GameObject obj = Instance._audioPool.GetAvailablePoolObject();
        if (obj == null) return null;

        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.SetParent(null, false);
        obj.transform.localPosition = position;
        obj.transform.rotation = Quaternion.identity;

        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = Instance._audioMixerGroupGameplay; // gameplayer audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();

        return audioSource;
    }

    public static AudioSource PlaySound(
        Transform parent,
        AudioClip clip,
        float volume = 1.0f,
        float pitch = 1.0f,
        float spatialBlend = 1.0f,
        float maxDistance = 30.0f)
    {
        GameObject obj = Instance._audioPool.GetAvailablePoolObject();
        if (obj == null) return null;

        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.SetParent(parent, false);

        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = Instance._audioMixerGroupGameplay; // gameplayer audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();

        return audioSource;
    }

    // used to set the outputAudioMixerGroup of an audio source
    public static void SetAudioSourceType(AudioSource audioSource, AudioType type)
    {
        switch (type)
        {
            case AudioType.Gameplay:
                audioSource.outputAudioMixerGroup = Instance._audioMixerGroupGameplay;
                break;
            case AudioType.Music:
                audioSource.outputAudioMixerGroup = Instance._audioMixerGroupMusic;
                break;
            case AudioType.Effect:
                audioSource.outputAudioMixerGroup = Instance._audioMixerGroupEffects;
                // Debug.LogError("Music and effect types can't be registered as audio sources. They have to be played by calling SoundManager.PlaySound()");
                break;
            default:
                Debug.LogError("Audio type " + type + " is not valid!");
                break;
        }
    }

    public static void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;

        // TODO: use master audio mixer instead
        // instance.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public static void SetVolume(AudioType type, float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f); // TODO: find if possible to clamp to 0 instead of 0.0001

        switch (type)
        {
            case AudioType.Gameplay:
                Instance._audioMixer.SetFloat("GameplayVolume", Mathf.Log10(volume) * 20);
                break;
            case AudioType.Music:
                Instance._audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
                break;
            case AudioType.Effect:
                Instance._audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
                break;
            default:
                Debug.LogError("Audio type " + type + " doesn't exist!");
                break;
        }
    }

    public static void TransitionToSnapshot(string snapshotName)
    {
        AudioMixerSnapshot snapshot = Instance._audioMixer.FindSnapshot(snapshotName);

        if (snapshot != null)
            snapshot.TransitionTo(0.2f);
        else
            Debug.LogError($"Can't find audioSnapshot '{snapshotName}'");
    }

    void ChangedActiveScene(Scene current, Scene next) // this is called after a scene has changed
    {
        _audioPool = new();
        _audioPool.Initialize();
    }
}
