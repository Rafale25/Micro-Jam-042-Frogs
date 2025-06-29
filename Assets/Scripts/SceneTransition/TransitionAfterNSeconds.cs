using System.Collections;
using UnityEngine;

public class TransitionAfterNSeconds : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private float _seconds;
    [SerializeField] private float _transitionDuration;

    void Start()
    {
        StartCoroutine(StartTransitionInNSeconds(_sceneName, _seconds, _transitionDuration));
    }

    IEnumerator StartTransitionInNSeconds(string sceneName, float seconds, float duration)
    {
        yield return new WaitForSeconds(seconds); // use WaitForSecondsRealtime ?,
        TransitionManager.Instance.TransitionToScene(sceneName, duration);
    }
}
