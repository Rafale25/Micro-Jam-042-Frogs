using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private CanvasGroup _canvasGroup; // used to do the fade

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
    }

    public void TransitionToScene(string sceneName, float duration, string transitionType = "fade")
    {
        StartCoroutine(FadeToScene(sceneName, duration));
    }

    void ChangedActiveScene(Scene current, Scene next) // this is called after a scene has changed
    {
        _canvasGroup = FindFirstObjectByType<CanvasGroup>();
    }

    IEnumerator FadeToScene(string sceneName, float duration)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float t = 0;

        while (op.progress < 0.9f || t < 1f)
        {
            t += Time.deltaTime * duration;
            t = Mathf.Clamp01(t);
            _canvasGroup.alpha = t;
            yield return null;
        }

        op.allowSceneActivation = true;
        yield return null;

        _canvasGroup.alpha = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * duration;
            t = Mathf.Clamp01(t);
            _canvasGroup.alpha = t;
            yield return null;
        }
    }
}
