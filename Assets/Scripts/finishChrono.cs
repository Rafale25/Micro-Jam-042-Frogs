using UnityEngine;

public class finishChrono : MonoBehaviour
{
    [SerializeField] private GameObject chrono;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControl>() != null)
        {
            chrono.GetComponent<chrono>().timerRunning = false;
        }
    }
}
