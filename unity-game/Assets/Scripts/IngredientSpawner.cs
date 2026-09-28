using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private FallingObject ingredientPrefab;
    [SerializeField, Min(0.01f)] private float spawnInterval = 1.5f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = 6f;

    private float elapsedTime;

    private void Awake()
    {
        if (ingredientPrefab == null || spawnInterval <= 0f || minX > maxX)
        {
            Debug.LogError("IngredientSpawner needs an Ingredient Prefab, Spawn Interval > 0, and Min X <= Max X.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < spawnInterval)
            return;

        // Spawn once per interval, without a burst after a slow frame.
        elapsedTime = 0f;
        Vector3 position = new Vector3(Random.Range(minX, maxX), spawnY, 0f);
        Instantiate(ingredientPrefab, position, Quaternion.identity);
    }
}
