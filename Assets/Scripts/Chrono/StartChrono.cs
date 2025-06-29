using UnityEngine;

public class StartChrono : MonoBehaviour
{
    [SerializeField] private Chrono _chrono;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControl>() != null)
        {
            _chrono.StartChrono();
        }
    }
}
