using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSequence : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private CanvasGroup _canvasGroup;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerControl playerControl))
        {
            playerControl.enabled = false;
            StartCoroutine(FadeToScene(_nextSceneName));
        }
    }

    IEnumerator FadeToScene(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float t = 0;

        while (op.progress < 0.9f || t < 1f)
        {
            t += Time.deltaTime / 2f;
            t = Mathf.Clamp01(t);
            _canvasGroup.alpha = t;
            yield return null;
        }

        op.allowSceneActivation = true;
    }
}
