using UnityEngine;

public class StopChrono : MonoBehaviour
{
    [SerializeField] private Chrono _chrono;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControl>() != null)
        {
            _chrono.StopChrono();
            _chrono.SaveHighscore();
        }
    }
}
