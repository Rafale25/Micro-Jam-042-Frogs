using UnityEngine;

public class BloodDrop : MonoBehaviour
{
    private ProceduralBloodTilemap _proceduralBloodTilemap;
    private Rigidbody2D _rb;

    void Awake()
    {
        _proceduralBloodTilemap = FindFirstObjectByType<ProceduralBloodTilemap>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D()
    {
        _proceduralBloodTilemap.SpawnBlood(transform.position, _rb.linearVelocity);
        Destroy(gameObject);
    }
}
