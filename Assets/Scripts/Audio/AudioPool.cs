using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    private const int _maxPoolSize = 32;

    private GameObject _poolParent;
    private Stack<GameObject> _audioSourcePool;
    private List<GameObject> _audioSourcePlaying;

    public int PoolCount => _audioSourcePool.Count;
    public int PlayingCount => _audioSourcePlaying.Count;

    public bool IsInitialized = false;

    public void Initialize()
    {
        if (IsInitialized) return;
        IsInitialized = true;

        _audioSourcePool = new Stack<GameObject>(_maxPoolSize);
        _audioSourcePlaying = new List<GameObject>();
        _poolParent = new GameObject("AudioSourcePool");

        for (int i = 0 ; i < _maxPoolSize ; ++i)
        {
            GameObject obj = new();
            obj.name = "AudioSourcePoolObject";
            obj.transform.SetParent(_poolParent.transform, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            AudioSource audioSource = obj.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = 1.0f;
            audioSource.maxDistance = 30f; // default max Distance

            _audioSourcePool.Push(obj);
        }
    }

    public void UpdatePool()
    {
        /* remove from audioSourcePlaying and add to audioSourcePool if isn't playing */
        _audioSourcePlaying.RemoveAll(obj => {

            if (obj == null) return true; /* if object has been destroyed by scene change, remove it */

            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                obj.transform.SetParent(_poolParent.transform, false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;

                _audioSourcePool.Push(obj);
                return true;
            }
            return false;
        });
    }

    public GameObject GetAvailablePoolObject()
    {
        if (_audioSourcePool.Count == 0)
        {
            Debug.LogError("Not enough element in audioSourcePool! " + _audioSourcePlaying.Count + " are currently being used.");
            return null;
        }

        GameObject obj = _audioSourcePool.Pop();
        _audioSourcePlaying.Add(obj);

        return obj;
    }

    public void StopAll()
    {
        _audioSourcePlaying.ForEach(obj => obj.GetComponent<AudioSource>().Stop());
        UpdatePool();
    }
}