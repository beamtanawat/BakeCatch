using UnityEngine;

public class ChefVisual : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        BakeryArt.Shape(transform, "Left shoe", new Vector2(-0.23f, -0.35f), new Vector2(0.37f, 0.18f), "#654438", 4);
        BakeryArt.Shape(transform, "Right shoe", new Vector2(0.23f, -0.35f), new Vector2(0.37f, 0.18f), "#654438", 4);
        BakeryArt.Shape(transform, "Apron", new Vector2(0, 0), new Vector2(0.75f, 0.7f), "#FFFAEE", 5);
        BakeryArt.Shape(transform, "Neckerchief", new Vector2(0, 0.22f), new Vector2(0.22f, 0.2f), "#ED6383", 6);
        BakeryArt.Shape(transform, "Hair", new Vector2(0, 0.62f), new Vector2(1, 0.85f), "#744535", 5);
        BakeryArt.Shape(transform, "Face", new Vector2(0, 0.58f), new Vector2(0.83f, 0.63f), "#FFDBB3", 6);
        for (int side = -1; side <= 1; side += 2)
        {
            BakeryArt.Shape(transform, "Eye", new Vector2(side * 0.2f, 0.65f), new Vector2(0.085f, 0.12f), "#4D3432", 7);
            BakeryArt.Shape(transform, "Blush", new Vector2(side * 0.3f, 0.48f), new Vector2(0.16f, 0.08f), "#F5A0A0", 7);
        }
        BakeryArt.Shape(transform, "Smile", new Vector2(0, 0.43f), new Vector2(0.15f, 0.045f), "#B26B5C", 7);
        BakeryArt.Shape(transform, "Chef hat", new Vector2(0, 1.08f), new Vector2(1.05f, 0.44f), "#FFF9EA", 8);
        BakeryArt.Shape(transform, "Hat puff", new Vector2(0, 1.33f), new Vector2(0.6f, 0.4f), "#FFF9EA", 8);
        BakeryArt.Shape(transform, "Catch tray", new Vector2(0, 1.64f), new Vector2(1.65f, 0.16f), "#FFC14D", 9);
        BakeryArt.Shape(transform, "Tray glow", new Vector2(0, 1.72f), new Vector2(1.55f, 0.04f), "#FFF7D1", 10);
    }
}
