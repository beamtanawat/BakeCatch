using UnityEngine;

public class BakeryBackdrop : MonoBehaviour
{
    private void Awake()
    {
        BakeryArt.Shape(transform, "Cream wallpaper", Vector2.zero, new Vector2(24, 12), "#F6DBC0", -50, false);
        for (int x = -9; x <= 9; x++)
            BakeryArt.Shape(transform, "Wallpaper stripe", new Vector2(x, 0), new Vector2(0.06f, 10), "#EDD0B5", -49, false);
        BakeryArt.Shape(transform, "Window frame", new Vector2(0, 1.1f), new Vector2(9, 5.4f), "#B88162", -48);
        BakeryArt.Shape(transform, "Morning sky", new Vector2(0, 1.1f), new Vector2(8.6f, 5), "#C7E7EB", -47);
        BakeryArt.Shape(transform, "Window cross", new Vector2(0, 1.1f), new Vector2(0.18f, 5), "#FFF1D8", -45);
        BakeryArt.Shape(transform, "Window cross", new Vector2(0, 1), new Vector2(8.6f, 0.18f), "#FFF1D8", -45);
        for (int i = 0; i < 12; i++)
            BakeryArt.Shape(transform, "Awning", new Vector2(-4.2f + i * 0.76f, 3.4f), new Vector2(0.78f, 0.85f), i % 2 == 0 ? "#F49B9E" : "#FFF1D8", -44);
        for (int side = -1; side <= 1; side += 2)
        {
            for (int shelf = 0; shelf < 3; shelf++)
            {
                float y = -1.5f + shelf * 1.6f;
                BakeryArt.Shape(transform, "Bakery shelf", new Vector2(side * 6.4f, y), new Vector2(3, 0.18f), "#BA805B", -43);
                for (int j = 0; j < 3; j++)
                {
                    float x = side * 6.4f - 0.9f + j * 0.9f;
                    BakeryArt.Shape(transform, "Pastry", new Vector2(x, y + 0.35f), new Vector2(0.65f, 0.55f), shelf % 2 == 0 ? "#D7A373" : "#F7ADC0", -42);
                    BakeryArt.Shape(transform, "Icing", new Vector2(x, y + 0.55f), new Vector2(0.5f, 0.16f), "#FFF4E3", -41);
                }
            }
        }
        BakeryArt.Shape(transform, "Floor", new Vector2(0, -4), new Vector2(24, 2), "#CD9876", -40, false);
        BakeryArt.Shape(transform, "Counter edge", new Vector2(0, -2.9f), new Vector2(24, 0.2f), "#A66B51", -39, false);
        for (int x = -9; x < 10; x++)
            BakeryArt.Shape(transform, "Floor seam", new Vector2(x, -4), new Vector2(0.025f, 2), "#B77E5E", -39, false);
    }
}
