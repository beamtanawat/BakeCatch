using UnityEngine;

public enum IngredientType { Strawberry, Milk, Egg, Cheese, Tomato, Dough, Bread, Meat, Chocolate, Golden, Hazard }

public sealed class IngredientDefinition
{
    public IngredientType Type { get; }
    public string Name { get; }
    public int Score { get; }
    public Color Color { get; }
    public bool IsHazard => Type == IngredientType.Hazard;
    public bool IsRecipeIngredient => Type != IngredientType.Golden && !IsHazard;

    private IngredientDefinition(IngredientType type, string name, int score, string color)
    {
        Type = type;
        Name = name;
        Score = score;
        ColorUtility.TryParseHtmlString(color, out Color parsed);
        Color = parsed;
    }

    private static readonly IngredientDefinition[] All =
    {
        new IngredientDefinition(IngredientType.Strawberry, "Strawberry", 10, "#F65379"),
        new IngredientDefinition(IngredientType.Milk, "Milk", 20, "#72C9FA"),
        new IngredientDefinition(IngredientType.Egg, "Egg", 10, "#FFE1AD"),
        new IngredientDefinition(IngredientType.Cheese, "Cheese", 30, "#FFC847"),
        new IngredientDefinition(IngredientType.Tomato, "Tomato", 10, "#F77860"),
        new IngredientDefinition(IngredientType.Dough, "Dough", 10, "#EAC791"),
        new IngredientDefinition(IngredientType.Bread, "Bread", 10, "#D49B65"),
        new IngredientDefinition(IngredientType.Meat, "Meat", 20, "#BB6D68"),
        new IngredientDefinition(IngredientType.Chocolate, "Chocolate", 30, "#9C6A54"),
        new IngredientDefinition(IngredientType.Golden, "Golden", 50, "#FFD84A"),
        new IngredientDefinition(IngredientType.Hazard, "Hazard", -20, "#55445B")
    };

    public static IngredientDefinition Get(IngredientType type) => All[(int)type];
}
