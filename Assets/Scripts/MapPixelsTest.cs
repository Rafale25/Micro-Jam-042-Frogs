using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPixelsTest : MonoBehaviour
{
    private const int pixelPerUnit = 16;

    private Texture2D _texture;
    private Tilemap _tilemap;

    void Awake()
    {
        _tilemap = FindFirstObjectByType<Tilemap>();
        _tilemap.CompressBounds();

        _texture = new Texture2D(_tilemap.size.x * pixelPerUnit, _tilemap.size.y * pixelPerUnit); // -2 because bounds extend one extra on each side
        _texture.filterMode = FilterMode.Point;


        for (int y = 0; y < _texture.height; ++y)
        for (int x = 0; x < _texture.width; ++x)
        {
                if (x % 2 == 0 && y % 2 == 1)
                {
                    _texture.SetPixel(x, y, Color.black);
                }
                else
                {
                    _texture.SetPixel(x, y, Color.white);

                }
        }
        _texture.Apply();

        var parent = new GameObject();
        var spriteRenderer = parent.AddComponent<SpriteRenderer>();
        var sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero, pixelPerUnit);
        spriteRenderer.sprite = sprite;

        parent.transform.position = new Vector3(_tilemap.origin.x, _tilemap.origin.y, 0);

        ShowTilemapTiles();
    }

    void ShowTilemapTiles()
    {
        BoundsInt area = _tilemap.cellBounds;
        TileBase[] tileArray = _tilemap.GetTilesBlock(area);

        print(_tilemap.tileAnchor);
        print(_tilemap.localBounds);
        print(_tilemap.origin);
        print(_tilemap.size);

        for (int y = 0; y < area.size.y; ++y)
        {
            for (int x = 0; x < area.size.x; ++x)
            {
                int index = y * area.size.x + x;

                if (tileArray[index] == null) continue;

                var p = _tilemap.tileAnchor.xy() + new Vector2(x + area.min.x, y + area.min.y);

                DebugDraw.DrawCircle(
                    position: p,
                    radius: 0.5f,
                    duration: 100f
                );
            }
        }
    }
}
