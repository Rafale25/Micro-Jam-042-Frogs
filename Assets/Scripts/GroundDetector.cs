using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool IsGrounded = false;

    void Awake()
    {
        IsGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IsGrounded = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        IsGrounded = false;
    }
}
