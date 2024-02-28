namespace RecipeManager.Models
{
    internal class RecipeBase
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public double Calories { get; set; }
        public string Comment { get; set; }

        public RecipeBase()
        {
        }
    }
}
