using UnityEngine;

public class BakeryBackdrop : MonoBehaviour
{
    private void Awake()
    {
        Sprite sprite = BakeryArt.Artwork("Bakery");
        Camera camera = Camera.main;
        float height = camera.orthographicSize * 2;
        float cover = Mathf.Max(height, height * camera.aspect / (sprite.bounds.size.x / sprite.bounds.size.y));
        BakeryArt.Illustration(transform, "Illustrated bakery", sprite, Vector2.zero, cover, -50);
    }
}
