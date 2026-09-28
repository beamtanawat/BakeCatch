using UnityEngine;

public sealed class SessionStatistics
{
    public int Score { get; private set; }
    public int CatchCount { get; private set; }
    public int MissCount { get; private set; }
    public int CurrentCombo { get; private set; }
    public int MaxCombo { get; private set; }
    public int RecipeComplete { get; private set; }
    public float Accuracy => CatchCount + MissCount == 0 ? 0f : 100f * CatchCount / (CatchCount + MissCount);
    public string ComboFeedback => CurrentCombo < 2 ? "Keep going!" : CurrentCombo == 2 ? "OK" :
        CurrentCombo == 3 ? "ดี" : CurrentCombo == 4 ? "ดีมาก" : CurrentCombo == 5 ? "ดีที่สุด" : "Combo x" + CurrentCombo;

    public void Resolve(IngredientDefinition ingredient, bool caught, RecipeProgress recipe)
    {
        if (ingredient.IsHazard)
        {
            if (caught)
            {
                Score = Mathf.Max(0, Score + ingredient.Score);
                CurrentCombo = 0;
            }
            return;
        }
        if (!caught)
        {
            MissCount++;
            CurrentCombo = 0;
            return;
        }

        CatchCount++;
        Score += ingredient.Score;
        CurrentCombo++;
        MaxCombo = Mathf.Max(MaxCombo, CurrentCombo);
        if (ingredient.IsRecipeIngredient && recipe.Collect(ingredient.Type)) RecipeComplete++;
    }
}

[System.Serializable]
public sealed class SessionResult
{
    public int score, catchCount, missCount, maxCombo, recipeComplete;
    public float durationSec, accuracy, leftAvgGrip, rightAvgGrip;
    public string level;

    public SessionResult(SessionStatistics stats, float duration, DifficultyLevel difficulty)
    {
        score = stats.Score;
        catchCount = stats.CatchCount;
        missCount = stats.MissCount;
        maxCombo = stats.MaxCombo;
        recipeComplete = stats.RecipeComplete;
        accuracy = stats.Accuracy;
        durationSec = duration;
        level = difficulty.ToString();
        // No sensor measurements exist in keyboard mode.
        leftAvgGrip = rightAvgGrip = 0f;
    }
}
