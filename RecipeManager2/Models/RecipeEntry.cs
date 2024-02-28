namespace RecipeManager.Models
{
    internal class RecipeEntry
    {
        public Ingredient Ingredient { get; set; }
        public double Amount { get; set; }

        public RecipeEntry(Ingredient ingredient, double amount)
        {
            Ingredient = ingredient;
            Amount = amount;
        }

        public RecipeEntry()
        {
        }
    }
}
