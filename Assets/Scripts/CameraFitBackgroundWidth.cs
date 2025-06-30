using Unity.Cinemachine;
using UnityEngine;

public class CameraFitBackgroundWidth : MonoBehaviour
{
    [SerializeField] private float _aspectRatioY = 4f;
    [SerializeField] private float _aspectRatioX = 3f;

    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    void Update()
    {
        // set camera width to map width
        float width = _backgroundSpriteRenderer.bounds.size.x;
        CinemachineCamera camera = GetComponent<CinemachineCamera>();
        float aspect = _aspectRatioY / _aspectRatioX;
        camera.Lens.OrthographicSize = (width / aspect) * 0.5f;
    }
}
