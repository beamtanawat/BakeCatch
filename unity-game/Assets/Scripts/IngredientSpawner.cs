using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private FallingObject ingredientPrefab;
    [SerializeField, Min(0.01f)] private float spawnInterval = 1.5f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = 6f;

    private float elapsedTime;
    private GameSession session;
    private readonly System.Collections.Generic.List<FallingObject> active = new System.Collections.Generic.List<FallingObject>();
    private int lane;

    public void SetSession(GameSession value) => session = value;
    public void ResetSpawning() { elapsedTime = 0f; lane = 0; }
    public void ClearObjects()
    {
        foreach (FallingObject item in active)
            if (item != null) { item.gameObject.SetActive(false); Destroy(item.gameObject); }
        active.Clear();
    }

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
        if (session != null && !session.IsPlaying) return;
        DifficultySettings settings = session != null ? DifficultySettings.Get(session.Difficulty) : null;
        active.RemoveAll(item => item == null || !item.gameObject.activeSelf);
        elapsedTime += Time.deltaTime;
        if (elapsedTime < (settings != null ? settings.SpawnInterval : spawnInterval))
            return;
        if (settings != null && active.Count >= settings.MaxObjects) return;

        // Spawn once per interval, without a burst after a slow frame.
        elapsedTime = 0f;
        float x = settings != null && settings.Predictable ? Mathf.Lerp(minX, maxX, (lane++ % 5) / 4f) : Random.Range(minX, maxX);
        Vector3 position = new Vector3(x, spawnY, 0f);
        FallingObject item = Instantiate(ingredientPrefab, position, Quaternion.identity);
        active.Add(item);
        if (session != null)
        {
            IngredientType type;
            if (Random.value < settings.HazardChance) type = IngredientType.Hazard;
            else if (Random.value < 0.7f) type = session.Recipe.NextNeeded();
            else type = (IngredientType)Random.Range(0, (int)IngredientType.Hazard);
            item.Configure(session, type, settings.FallSpeed);
        }
    }
}
