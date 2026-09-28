using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BakeCatchSetup
{
    [MenuItem("Bake Catch/Prepare Standalone Game")]
    public static void Prepare()
    {
        if (EditorApplication.isPlaying) throw new System.InvalidOperationException("Exit Play Mode before setup.");
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        IngredientSpawner spawner = Object.FindFirstObjectByType<IngredientSpawner>();
        Font font = AssetDatabase.LoadAssetAtPath<Font>("Assets/UI/Fonts/NotoSansThai.ttf");
        if (player == null || spawner == null || font == null) throw new System.InvalidOperationException("Player, spawner, and font must exist.");

        GameObject prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Ingredient.prefab");
        try
        {
            Rigidbody2D body = GetOrAdd<Rigidbody2D>(prefab);
            body.bodyType = RigidbodyType2D.Kinematic;
            body.useFullKinematicContacts = true;
            body.gravityScale = 0f;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            CircleCollider2D collider = GetOrAdd<CircleCollider2D>(prefab);
            collider.radius = 0.44f;
            collider.isTrigger = true;
            prefab.transform.localScale = Vector3.one;
            Assign(GetOrAdd<IngredientVisual>(prefab), "labelFont", font);
            PrefabUtility.SaveAsPrefabAsset(prefab, "Assets/Prefabs/Ingredient.prefab");
        }
        finally { PrefabUtility.UnloadPrefabContents(prefab); }

        // Keep the original scene placeholder as an inactive reference, not a stray live item.
        foreach (FallingObject item in Object.FindObjectsByType<FallingObject>(FindObjectsSortMode.None))
            if (item.gameObject.scene == scene) item.gameObject.SetActive(false);

        GameObject sessionObject = GameObject.Find("Game Session") ?? new GameObject("Game Session");
        GameSession session = GetOrAdd<GameSession>(sessionObject);
        Assign(session, "player", player);
        Assign(session, "spawner", spawner);
        // Reserve the left side for the order card; all spawn lanes stay readable.
        SerializedObject spawnSettings = new SerializedObject(spawner);
        spawnSettings.FindProperty("minX").floatValue = -4.5f;
        spawnSettings.FindProperty("maxX").floatValue = 7f;
        spawnSettings.ApplyModifiedPropertiesWithoutUndo();
        GetOrAdd<ChefVisual>(player.gameObject);
        Transform catchTransform = player.transform.Find("Catch Zone");
        GameObject catchObject = catchTransform != null ? catchTransform.gameObject : new GameObject("Catch Zone");
        catchObject.transform.SetParent(player.transform, false);
        catchObject.transform.localPosition = new Vector3(0, 1.72f, 0);
        BoxCollider2D zone = GetOrAdd<BoxCollider2D>(catchObject);
        zone.isTrigger = true;
        zone.size = new Vector2(1.65f, 0.25f);
        GetOrAdd<CatchZone>(catchObject);

        GameObject background = GameObject.Find("Bakery Background") ?? new GameObject("Bakery Background");
        GetOrAdd<BakeryBackdrop>(background);
        GameObject ui = GameObject.Find("Bake Catch UI") ?? new GameObject("Bake Catch UI", typeof(RectTransform));
        BakeCatchUI screens = GetOrAdd<BakeCatchUI>(ui);
        Assign(screens, "session", session);
        Assign(screens, "font", font);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("BAKE_CATCH_SETUP_OK: Game scene and Ingredient prefab configured.");
    }

    private static T GetOrAdd<T>(GameObject target) where T : Component
    {
        T component = target.GetComponent<T>();
        return component != null ? component : target.AddComponent<T>();
    }
    private static void Assign(Object target, string field, Object value)
    {
        SerializedObject serialized = new SerializedObject(target);
        serialized.FindProperty(field).objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
}
