using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _player = null;
    private Vector2 _playerOriginalPosition = Vector2.zero;
    private Chrono _chrono = null;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _player = FindFirstObjectByType<PlayerControl>().gameObject;
        _playerOriginalPosition = _player.transform.position;
        _chrono = FindFirstObjectByType<Chrono>();
    }

    public void Restart(float delay = 0f)
    {
        Instance.Invoke(
            () => TransitionManager.Instance.Transition(0.2f, func: () => Instance.Reset()),
            delay
        );
    }

    public void Reset()
    {
        _player.transform.position = _playerOriginalPosition;
        _player.GetComponent<PlayerControl>().ResetPlayerToDefault();

        _chrono.StopChrono();
        _chrono.ResetChrono();
    }
}
