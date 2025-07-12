using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPixelsTest : MonoBehaviour
{
    [SerializeField] private int _orderInLayer = 0;

    private const int pixelPerUnit = 16;

    private Texture2D _texture;
    private Tilemap _tilemap;

    private int _textureWidth, _textureHeight;

    private bool[] _textureBitmapMask = null;

    void Awake()
    {
        _tilemap = FindFirstObjectByType<Tilemap>();
        _tilemap.CompressBounds();

        // print(_tilemap.tileAnchor);
        // print(_tilemap.localBounds);
        // print(_tilemap.origin);
        // print(_tilemap.size);

        _textureWidth = _tilemap.size.x * pixelPerUnit;
        _textureHeight = _tilemap.size.y * pixelPerUnit;
        _texture = new(_textureWidth, _textureHeight) {
            filterMode = FilterMode.Point
        };

        _textureBitmapMask = new bool[_textureWidth * _textureHeight];
        for (int y = 0; y < _textureHeight; ++y)
        {
            for (int x = 0; x < _textureWidth; ++x)
            {
                int index = y * _textureWidth + x;
                int localX = x % pixelPerUnit;
                int localY = y % pixelPerUnit;

                var cellPos = _tilemap.origin + new Vector3Int((int)((float)x / pixelPerUnit), (int)((float)y / pixelPerUnit));
                // _textureBitmapMask[index] = _tilemap.GetTile(cellPos) != null;
                var sp = _tilemap.GetSprite(cellPos);
                // sp.texture.isR
                _textureBitmapMask[index] = sp != null && sp.texture.GetPixel(localX, localY) != Color.clear;

                // I WAS HERE : WTF ?
            }
        }


        for (int y = 0; y < _textureHeight; ++y)
        {
            for (int x = 0; x < _textureWidth; ++x)
            {
                // _texture.SetPixel(x, y, (x % 2 == 0 && y % 2 == 1) ? Color.black : Color.white);
                int index = y * _textureWidth + x;
                _texture.SetPixel(x, y, _textureBitmapMask[index] ? Color.white : Color.black);

                // _texture.SetPixel(x, y, Color.clear);
            }
        }
        _texture.Apply();

        var sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero, pixelPerUnit);
        var parent = new GameObject();
        var spriteRenderer = parent.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        // spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        spriteRenderer.sortingOrder = _orderInLayer;

        parent.transform.position = new Vector3(_tilemap.origin.x, _tilemap.origin.y, 0);

        // ShowTilemapTiles();
    }

    void Update()
    {
        // Vector2 playerPosition = FindFirstObjectByType<PlayerControl>().transform.position;
        // playerPosition.y -= 1f;
        // SpawnBlood(playerPosition);
    }

    public void SpawnBlood(Vector2 worldPosition)
    {
        Vector2Int p = WorldPositionToTextureLocalPosition(worldPosition);

        const int radius = 5;
        for (int y = 0; y < radius * 2; ++y)
        {
            for (int x = 0; x < radius * 2; ++x)
            {
                int px = p.x + x - radius;
                int py = p.y + y - radius;
                // if in radius
                if (!IsInsideTextureBounds(px, py)) continue;
                _texture.SetPixel(px, py, Color.red);
            }
        }

        _texture.Apply(); //updateMipmaps: false);
    }

    bool IsInsideTextureBounds(int x, int y)
    {
        return !(x < 0 || x >= _textureWidth || y < 0 || y >= _textureHeight);
    }

    Vector2Int WorldPositionToTextureLocalPosition(Vector2 worldPosition)
    {
        worldPosition -= new Vector2(_tilemap.origin.x, _tilemap.origin.y);
        worldPosition *= pixelPerUnit;

        return new Vector2Int((int)worldPosition.x, (int)worldPosition.y);
    }

    void ShowTilemapTiles()
    {
        BoundsInt area = _tilemap.cellBounds;
        TileBase[] tileArray = _tilemap.GetTilesBlock(area);

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
