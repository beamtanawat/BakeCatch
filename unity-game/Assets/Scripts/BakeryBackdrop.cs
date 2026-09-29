using UnityEngine;

public class BakeryBackdrop : MonoBehaviour
{
    private SpriteRenderer backdrop;
    private Camera gameCamera;
    private float previousAspect, previousHeight;

    private void Awake()
    {
        Sprite sprite = BakeryArt.Artwork("Bakery");
        gameCamera = Camera.main;
        backdrop = BakeryArt.Illustration(transform, "Illustrated bakery", sprite, Vector2.zero, 10, -50);
        LateUpdate();
    }

    private void LateUpdate()
    {
        float height = gameCamera.orthographicSize * 2;
        float aspect = gameCamera.aspect;
        if (height == previousHeight && aspect == previousAspect) return;
        Vector2 size = backdrop.sprite.bounds.size;
        float scale = Mathf.Max(height / size.y, height * aspect / size.x);
        backdrop.transform.localScale = Vector3.one * scale;
        previousHeight = height;
        previousAspect = aspect;
    }
}
