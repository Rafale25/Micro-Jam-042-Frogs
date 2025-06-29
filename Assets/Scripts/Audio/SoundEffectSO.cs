using UnityEngine;

// using UnityEditor;
// #if UNITY_EDITOR
// [CustomEditor(typeof(SoundEffectSO))]
// public class TestScriptableEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         var script = (SoundEffectSO)target;

//         EditorGUILayout.MinMaxSlider(ref script.volume.x, ref script.volume.y, 0f, 1f);
//         EditorGUILayout.MinMaxSlider(ref script.pitch.x, ref script.pitch.y, 0f, 2f);

//         // if(GUILayout.Button("Button", GUILayout.Height(40)))
//         // {
//         //     // script.UpdateCounter();
//         // }

//     }
// }
// #endif

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    public AudioClip[] clips;

    public Vector2 volume = new(1f, 1f);
    public Vector2 pitch = new(1f, 1f);
    [Range(0f, 1f)] public float spatialBlend = 0f;

    // OrderType order;

    private AudioClip GetAudioClip()
    {
        return clips[ Random.Range(0, clips.Length) ];
    }

    public AudioSource Play(Transform parent)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning($"Missing sound clips for {name}");
            return null;
        }

        return AudioManager.PlaySound(
            parent: parent,
            clip: GetAudioClip(),
            volume: Random.Range(volume.x, volume.y),
            pitch: Random.Range(pitch.x, pitch.y),
            spatialBlend: spatialBlend
        );
    }

    public AudioSource Play(Vector3 position = default)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning($"Missing sound clips for {name}");
            return null;
        }

        return AudioManager.PlaySound(
            position: position,
            clip: GetAudioClip(),
            volume: Random.Range(volume.x, volume.y),
            pitch: Random.Range(pitch.x, pitch.y),
            spatialBlend: spatialBlend
        );
    }

// #if UNITY_EDITOR
//     private AudioSource previewer;

//     private void OnEnable()
//     {
//         previewer = EditorUtility
//             .CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave,
//                 typeof(AudioSource))
//             .GetComponent<AudioSource>();
//     }

//     private void OnDisable()
//     {
//         DestroyImmediate(previewer.gameObject);
//     }

//     private void PlayPreview()
//     {
//         Play(position: Vector3.zero, spatialBlend: 0f);
//     }

//     private void StopPreview()
//     {
//         previewer.Stop();
//     }
// #endif

}
