using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField, Min(0f)] private float fallSpeed = 2.5f;
    [SerializeField] private float destroyY = -6f;
    [SerializeField] private IngredientType ingredientType;
    private GameSession session;
    private bool resolved;
    private Rigidbody2D body;
    public IngredientDefinition Definition => IngredientDefinition.Get(ingredientType);

    public void Configure(GameSession owner, IngredientType type, float speed)
    {
        session = owner;
        ingredientType = type;
        fallSpeed = speed;
        GetComponent<SpriteRenderer>().color = Definition.Color;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (body != null) return;
        Move(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (body != null) Move(Time.fixedDeltaTime);
    }

    private void Move(float deltaTime)
    {
        if (resolved || (session != null && !session.IsPlaying)) return;
        Vector3 position = transform.position;
        position.y -= fallSpeed * deltaTime;
        if (body != null) body.MovePosition(position);
        else transform.position = position;

        if (position.y < destroyY)
            Resolve(false);
    }

    public bool TryCatch() => Resolve(true);

    private bool Resolve(bool caught)
    {
        if (resolved || (session != null && !session.IsPlaying)) return false;
        resolved = true;
        if (session != null) session.Resolve(Definition, caught);
        gameObject.SetActive(false);
        Destroy(gameObject);
        return true;
    }
}
