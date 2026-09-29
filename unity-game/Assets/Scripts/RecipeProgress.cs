public sealed class RecipeDefinition
{
    public string Name { get; }
    public IngredientType[] Ingredients { get; }
    public int[] Quantities { get; }

    public RecipeDefinition(string name, IngredientType[] ingredients, int[] quantities)
    {
        Name = name;
        Ingredients = ingredients;
        Quantities = quantities;
    }
}

public sealed class RecipeProgress
{
    private static readonly RecipeDefinition[] Recipes =
    {
        new RecipeDefinition("Strawberry Cake", new[] { IngredientType.Strawberry, IngredientType.Milk, IngredientType.Egg }, new[] { 1, 1, 1 }),
        new RecipeDefinition("Pizza", new[] { IngredientType.Cheese, IngredientType.Tomato, IngredientType.Dough }, new[] { 1, 1, 1 }),
        new RecipeDefinition("Burger", new[] { IngredientType.Bread, IngredientType.Meat, IngredientType.Cheese }, new[] { 1, 1, 1 })
    };

    private int recipeIndex;
    public RecipeDefinition Current => Recipes[recipeIndex];
    public int[] Collected { get; private set; } = new int[3];

    // Return completion once, then move to a fresh order.
    public bool Collect(IngredientType type)
    {
        for (int i = 0; i < Current.Ingredients.Length; i++)
        {
            if (Current.Ingredients[i] == type && Collected[i] < Current.Quantities[i])
            {
                Collected[i]++;
                break;
            }
        }
        for (int i = 0; i < Collected.Length; i++)
            if (Collected[i] < Current.Quantities[i]) return false;

        recipeIndex = (recipeIndex + 1) % Recipes.Length;
        Collected = new int[Current.Ingredients.Length];
        return true;
    }

    public IngredientType NextNeeded()
    {
        for (int i = 0; i < Collected.Length; i++)
            if (Collected[i] < Current.Quantities[i]) return Current.Ingredients[i];
        return Current.Ingredients[0];
    }
}
