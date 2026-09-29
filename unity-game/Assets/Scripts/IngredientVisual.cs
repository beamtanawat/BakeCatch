using UnityEngine;

public class IngredientVisual : MonoBehaviour
{
    // Retained for existing prefab serialization; gameplay does not depend on labels.
    [SerializeField] private Font labelFont;

    private void Start()
    {
        IngredientDefinition definition = GetComponent<FallingObject>().Definition;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = BakeryArt.Icon((int)definition.Type);
        sprite.color = Color.white;
        sprite.sortingOrder = 15;
        // Preserve the validated collider scale exactly.
        transform.localScale = new Vector3(.72f, .72f, 1);
    }
}
