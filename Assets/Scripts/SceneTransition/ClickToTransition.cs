using UnityEngine;

public class ClickToTransition : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private float _transitionDuration;

    private bool _isTransitioning = false;

    void OnLeftClick()
    {
        if (_isTransitioning) return;
        _isTransitioning = true;

        TransitionManager.Instance.TransitionToScene(_sceneName, _transitionDuration);
    }
}
