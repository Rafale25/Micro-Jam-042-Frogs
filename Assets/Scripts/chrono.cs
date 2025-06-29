using TMPro;
using UnityEngine;

public class chrono : MonoBehaviour
{
    [SerializeField] private float chronoTimer = 0;
    [SerializeField] private int chronoDisplay = 0;
    [SerializeField] private BoxCollider2D startChrono;

    [SerializeField] public bool timerRunning = false;
    [SerializeField] public TMP_Text timerText;


    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            chronoTimer += Time.deltaTime;

            timerText.SetText(chronoTimer.ToString("F1"));
        }
    }


}
