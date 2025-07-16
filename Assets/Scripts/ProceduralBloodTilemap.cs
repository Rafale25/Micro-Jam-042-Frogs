using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

/*
- simplify code
- player procedural blood
- player procedural sprite cut
*/

public class ProceduralBloodTilemap : MonoBehaviour
{
    [SerializeField] private int _orderInLayer = 0;

    private const int pixelPerUnit = 16;

    private Texture2D _texture;
    private Tilemap _tilemap;
    private int _textureWidth, _textureHeight;
    private bool[] _textureBitmapMask = null;

    class Bloodrop
    {
        public Vector2Int position;
        public int amount;

        public Bloodrop(int px, int py, int amount)
        {
            this.position = new(px, py);
            this.amount = amount;
        }
    }

    private List<Bloodrop> _bloodDrops = new();

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
        _texture = new(_textureWidth, _textureHeight)
        {
            filterMode = FilterMode.Point
        };

        CreateBitMaskMap();
        ClearTexture();
        CreateSpriteObject();
        // ShowTilemapTiles();

        StartCoroutine(BloodDropsCoroutine());
    }

    void CreateBitMaskMap()
    {
        _textureBitmapMask = new bool[_textureWidth * _textureHeight];
        for (int y = 0; y < _textureHeight; ++y)
        {
            for (int x = 0; x < _textureWidth; ++x)
            {
                int index = y * _textureWidth + x;
                int localX = x % pixelPerUnit;
                int localY = y % pixelPerUnit;

                var cellPos = _tilemap.origin + new Vector3Int((int)((float)x / pixelPerUnit), (int)((float)y / pixelPerUnit));
                var sp = _tilemap.GetSprite(cellPos);
                _textureBitmapMask[index] = sp != null && sp.texture.GetPixel((int)sp.textureRect.x + localX, (int)sp.textureRect.y + localY).a > 0.1f;
            }
        }
    }

    void ClearTexture()
    {
        for (int y = 0; y < _textureHeight; ++y)
        {
            for (int x = 0; x < _textureWidth; ++x)
            {
                int index = y * _textureWidth + x;
                // _texture.SetPixel(x, y, (x % 2 == 0 && y % 2 == 1) ? Color.black : Color.white);
                // _texture.SetPixel(x, y, _textureBitmapMask[index] ? Color.white : Color.black);
                _texture.SetPixel(x, y, Color.clear);
            }
        }
        _texture.Apply();
    }

    void CreateSpriteObject()
    {
        var sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero, pixelPerUnit);
        var parent = new GameObject();
        var spriteRenderer = parent.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        spriteRenderer.sortingOrder = _orderInLayer;
        parent.transform.position = new Vector3(_tilemap.origin.x, _tilemap.origin.y, 0);
        // DontDestroyOnLoad(parent);
        // DontDestroyOnLoad(this);
    }

    void Update()
    {
        try
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int p = WorldPositionToTextureLocalPosition(mouseWorld);

            SpawnBlood(mouseWorld, Input.mousePositionDelta.xy()); // use mouse delta for DEBUGGING

            // var cellPos = _tilemap.origin + new Vector3Int((int)((float)p.x / pixelPerUnit), (int)((float)p.y / pixelPerUnit));
            // var sp = _tilemap.GetSprite(cellPos);
            // if (sp != null)
            // {
            //     print(" ");
            //     print(sp.texture.name);
            //     print(sp.rect);
            //     print(sp.textureRect);
            //     print(sp.textureRect);
            //     // print(sp.texture.GetPixel(localX, localY));
            // }

            // _texture.SetPixel(p.x, p.y, Color.red);
            // _texture.Apply(updateMipmaps: false);
        }
        catch
        {
            return;
        }

        // Vector2Int p = WorldPositionToTextureLocalPosition(worldPosition);
        // Vector2 playerPosition = FindFirstObjectByType<PlayerControl>().transform.position;
        // playerPosition.y -= 1f;
        // SpawnBlood(playerPosition);
    }

    bool DrawBloodPixel(int x, int y)
    {
        if (!IsInsideTextureBounds(x, y)) return false;
        if (_textureBitmapMask[y * _textureWidth + x] == false) return false;
        _texture.SetPixel(x, y, Color.red);
        return true;
    }

    // chatGPT generated function
    Vector2 StretchAlongDirection(Vector2 point, Vector2 direction, float scale)
    {
        if (direction == Vector2.zero)
            return point;

        Vector2 unitDir = direction.normalized;

        // Project the point onto the direction vector
        float projLength = Vector2.Dot(point, unitDir);
        Vector2 projection = projLength * unitDir;

        // Compute the orthogonal (perpendicular) component
        Vector2 perpendicular = point - projection;

        // Stretch only the projection
        Vector2 stretchedProjection = projection * scale;

        // Return recombined result
        return (stretchedProjection + perpendicular) / scale;
    }

    public void SpawnBlood(Vector2 worldPosition, Vector2 velocity)
    {
        Vector2Int p = WorldPositionToTextureLocalPosition(worldPosition);

        velocity = Vector2.Perpendicular(velocity);

        const int radius = 6;
        for (int y = -radius*2; y < radius*2; ++y)
        {
            for (int x = -radius*2; x < radius*2; ++x)
            {
                int px = p.x + x;
                int py = p.y + y;

                Vector2 point = new(x + 0.5f, y + 0.5f);

                const float strechScale = 3f;
                var stretchedPoint = StretchAlongDirection(point, velocity, strechScale);

                float sqrDist = stretchedPoint.sqrMagnitude;// point.sqrMagnitude;
                if (sqrDist > radius * radius) continue; // make the bllod mark round
                if (Random.value * radius * radius < sqrDist) continue; // make the blood mark more natural with some random empty spots

                if (DrawBloodPixel(px, py)) // if pixel was drawn
                {
                    if (Random.value < 1f / 5f) //  20% chance of spawning a dripping blood-drop
                    {
                        _bloodDrops.Add(new Bloodrop(px, py, Random.Range(4, 10)));
                    }
                }

            }
        }

        _texture.Apply(updateMipmaps: false);
    }

    IEnumerator BloodDropsCoroutine()
    {
        while (true)
        {
            _bloodDrops.RemoveAll(x => x.amount <= 0);

            foreach (var bd in _bloodDrops)
            {
                DrawBloodPixel(bd.position.x, bd.position.y);
                bd.amount -= 1;
                bd.position.y -= 1;
            }

            _texture.Apply(updateMipmaps: false);
            yield return new WaitForSeconds(0.2f);
        }
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
