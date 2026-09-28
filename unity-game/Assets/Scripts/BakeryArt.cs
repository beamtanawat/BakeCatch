using UnityEngine;

// Simple original geometric placeholders; replace their sprites with final art later.
public static class BakeryArt
{
    private static Sprite round;
    private static Sprite square;
    public static Sprite Square => square != null ? square : square = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    public static Sprite Round
    {
        get
        {
            if (round != null) return round;
            const int size = 64;
            Texture2D texture = new Texture2D(size, size);
            texture.name = "Rounded bakery placeholder";
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float dx = Mathf.Max(Mathf.Abs(x - 31.5f) - 19.5f, 0);
                    float dy = Mathf.Max(Mathf.Abs(y - 31.5f) - 19.5f, 0);
                    texture.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(12f - Mathf.Sqrt(dx * dx + dy * dy))));
                }
            texture.Apply();
            round = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 64, 0, SpriteMeshType.FullRect, new Vector4(16, 16, 16, 16));
            return round;
        }
    }

    public static Color Hex(string value) { ColorUtility.TryParseHtmlString(value, out Color color); return color; }

    public static GameObject Shape(Transform parent, string name, Vector2 position, Vector2 size, string color, int order, bool rounded = true)
    {
        GameObject item = new GameObject(name);
        item.transform.SetParent(parent, false);
        item.transform.localPosition = new Vector3(position.x, position.y, 0);
        item.transform.localScale = new Vector3(size.x, size.y, 1);
        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = rounded ? Round : Square;
        renderer.color = Hex(color);
        renderer.sortingOrder = order;
        return item;
    }
}
