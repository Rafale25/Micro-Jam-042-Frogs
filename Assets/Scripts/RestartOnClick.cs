using UnityEngine;
using UnityEngine.InputSystem;

public class RestartOnClick : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _actionMiddleClick;

    void Awake()
    {
        _playerInput = FindFirstObjectByType<PlayerInput>();
        _actionMiddleClick = _playerInput.actions["MiddleClick"];
    }

    void OnEnable()
    {
        _actionMiddleClick.performed += MiddleClick;
    }

    void OnDisable()
    {
        _actionMiddleClick.performed -= MiddleClick;
    }

    void MiddleClick(InputAction.CallbackContext context)
    {
        TransitionManager.Instance.TransitionToScene("Game", 0.25f);
        HighscoreStore.MadeNewHighscore = false;
    }
}
