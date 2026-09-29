using UnityEngine;

// Presentation-only assets. Replace PNGs without changing game rules or colliders.
public static class BakeryArt
{
    private static readonly System.Collections.Generic.Dictionary<string, Sprite> art = new System.Collections.Generic.Dictionary<string, Sprite>();
    public static Sprite Artwork(string name)
    {
        if (art.TryGetValue(name, out Sprite cached) && cached != null) return cached;
        Texture2D texture = Resources.Load<Texture2D>("BakeCatch/" + name);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100);
        sprite.name = name;
        art[name] = sprite;
        return sprite;
    }

    public static Sprite Icon(int index)
    {
        string name = "Icon " + index;
        if (art.TryGetValue(name, out Sprite cached) && cached != null) return cached;
        Texture2D texture = Resources.Load<Texture2D>("BakeCatch/Ingredients");
        // Hand-inspected transparent gutters preserve the complete icon silhouettes.
        float[] rows = { 0, 328, 617, 924, 1254 };
        float[] columns = { 0, 324, 627, 940, 1254 };
        int x = index % 4, y = index / 4;
        float scale = texture.width / 1254f;
        Rect rect = new Rect(columns[x] * scale, (1254 - rows[y + 1]) * scale,
            (columns[x + 1] - columns[x]) * scale, (rows[y + 1] - rows[y]) * scale);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), rect.height);
        sprite.name = name;
        art[name] = sprite;
        return sprite;
    }

    public static SpriteRenderer Illustration(Transform parent, string name, Sprite sprite, Vector2 position, float height, int order)
    {
        GameObject item = new GameObject(name);
        item.transform.SetParent(parent, false);
        item.transform.localPosition = position;
        item.transform.localScale = Vector3.one * (height / sprite.bounds.size.y);
        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = order;
        return renderer;
    }
    private static Sprite round;
    private static Sprite square;
    private static Sprite circle;
    public static Sprite Circle
    {
        get
        {
            if (circle != null) return circle;
            Texture2D texture = new Texture2D(128, 128);
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                    texture.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(63 - Vector2.Distance(new Vector2(x, y), new Vector2(63.5f, 63.5f)))));
            texture.Apply();
            circle = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(.5f, .5f), 128);
            return circle;
        }
    }
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
