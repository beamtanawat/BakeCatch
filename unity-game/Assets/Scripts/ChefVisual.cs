using UnityEngine;

public class ChefVisual : MonoBehaviour
{
    private Material outlineMaterial;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        BakeryArt.Illustration(transform, "Chibi chef", BakeryArt.Artwork("Chef"), new Vector2(0, .35f), 2.65f, 8);
        // The ring traces the existing collider, not a new or enlarged catch area.
        outlineMaterial = new Material(Shader.Find("Sprites/Default"));
        Ring("Catch rim", .06f, Color.white, 9);
        Ring("Pink catch rim", .026f, BakeryArt.Hex("#FF568C"), 10);
    }

    private void Ring(string name, float width, Color color, int order)
    {
        GameObject rim = new GameObject(name);
        rim.transform.SetParent(transform, false);
        LineRenderer line = rim.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.sharedMaterial = outlineMaterial;
        line.startColor = line.endColor = color;
        line.startWidth = line.endWidth = width;
        line.sortingOrder = order;
        line.loop = true;
        line.positionCount = 64;
        for (int i = 0; i < 64; i++)
        {
            float angle = i * Mathf.PI * 2 / 64;
            line.SetPosition(i, new Vector3(Mathf.Cos(angle) * .825f, 1.72f + Mathf.Sin(angle) * .125f, 0));
        }
    }

    private void OnDestroy() { if (outlineMaterial != null) Destroy(outlineMaterial); }
}
