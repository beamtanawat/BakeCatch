using UnityEngine;

public class IngredientVisual : MonoBehaviour
{
    [SerializeField] private Font labelFont;
    private void Start()
    {
        IngredientDefinition definition = GetComponent<FallingObject>().Definition;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = BakeryArt.Round;
        sprite.color = definition.Color;
        transform.localScale = new Vector3(0.72f, 0.72f, 1);
        GameObject label = new GameObject("Ingredient name");
        label.transform.SetParent(transform, false);
        label.transform.localPosition = new Vector3(0, -0.75f, 0);
        TextMesh text = label.AddComponent<TextMesh>();
        text.font = labelFont;
        text.fontSize = 64;
        text.characterSize = 0.065f;
        text.anchor = TextAnchor.MiddleCenter;
        text.text = definition.Name;
        text.color = BakeryArt.Hex("#593C39");
        MeshRenderer mesh = label.GetComponent<MeshRenderer>();
        mesh.sharedMaterial = labelFont.material;
        mesh.sortingOrder = 15;

        string mark = definition.IsHazard ? "!" : definition.Type == IngredientType.Golden ? "+50" : definition.Name.Substring(0, 1);
        GameObject symbol = new GameObject("Ingredient symbol");
        symbol.transform.SetParent(transform, false);
        TextMesh icon = symbol.AddComponent<TextMesh>();
        icon.font = labelFont;
        icon.fontSize = 64;
        icon.characterSize = 0.11f;
        icon.anchor = TextAnchor.MiddleCenter;
        icon.text = mark;
        icon.color = definition.IsHazard ? Color.white : BakeryArt.Hex("#593C39");
        symbol.GetComponent<MeshRenderer>().sharedMaterial = labelFont.material;
        symbol.GetComponent<MeshRenderer>().sortingOrder = 16;
    }
}
