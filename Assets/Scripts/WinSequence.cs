using UnityEngine;
using UnityEngine.InputSystem;

public class WinSequence : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            playerInput.enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity *= 0.4f; // reduce velocity of player so he doesn't fall after the win condition
            TransitionManager.Instance.TransitionToScene(_nextSceneName, 1.5f);
        }
    }
}
