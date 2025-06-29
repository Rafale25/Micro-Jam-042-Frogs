using Unity.Cinemachine;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    // [SerializeField] private float _aspectRatioY = 4f;
    // [SerializeField] private float _aspectRatioX = 3f;

    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

    // void Start()
    // {
    //     // set the desired aspect ratio (the values in this example are
    //     // hard-coded for 4:3, but you could make them into public
    //     // variables instead so you can set them at design time)
    //     float targetaspect = _aspectRatioY / _aspectRatioX;

    //     // determine the game window's current aspect ratio
    //     float windowaspect = (float)Screen.width / (float)Screen.height;

    //     // current viewport height should be scaled by this amount
    //     float scaleheight = windowaspect / targetaspect;

    //     // obtain camera component so we can modify its viewport
    //     Camera camera = GetComponent<Camera>();

    //     // if scaled height is less than current height, add letterbox
    //     if (scaleheight < 1.0f)
    //     {
    //         Rect rect = camera.rect;

    //         rect.width = 1.0f;
    //         rect.height = scaleheight;
    //         rect.x = 0;
    //         rect.y = (1.0f - scaleheight) / 2.0f;

    //         camera.rect = rect;
    //     }
    //     else // add pillarbox
    //     {
    //         float scalewidth = 1.0f / scaleheight;

    //         Rect rect = camera.rect;

    //         rect.width = scalewidth;
    //         rect.height = 1.0f;
    //         rect.x = (1.0f - scalewidth) / 2.0f;
    //         rect.y = 0;

    //         camera.rect = rect;
    //     }
    // }

    void Update()
    {
        float width = _backgroundSpriteRenderer.bounds.size.x;

        CinemachineCamera camera = GetComponent<CinemachineCamera>();

        float aspect = (float)Screen.width / Screen.height;
        // camera.orthographic = true;
        camera.Lens.OrthographicSize = (width / aspect) * 0.5f;
    }
}
