using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private CanvasGroup _canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeToScene(_nextSceneName,3));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator waitForGame(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        SceneManager.LoadScene("Game");
    }

    IEnumerator FadeToScene(string sceneName, int delay)
    {
        yield return new WaitForSeconds(delay);
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
