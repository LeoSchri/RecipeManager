using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Models
{
    internal class Recipe : RecipeBase
    {
        public List<RecipeEntry> Ingredients { get; set; }
        public string OrderedListOfIngredients
        {
            get
            {
                if (Ingredients != null && Ingredients.Any())
                {
                    var tempIngredients = Ingredients.OrderByDescending(x => x.Amount).ToList();
                    var ingredientNames = new List<string>();
                    tempIngredients.ForEach(i => ingredientNames.Add(i.Ingredient.Name));
                    return string.Join(", ", ingredientNames);
                }
                else return string.Empty;
            }
        }
        public new int Calories
        {
            get
            {
                if (Ingredients != null && Ingredients.Any())
                {
                    var sumCalories = 0.0;
                    foreach (RecipeEntry entry in Ingredients)
                    {
                        sumCalories += entry.Ingredient.Calories * entry.Amount;
                    }
                    var sumAmount = Ingredients.Sum(x => x.Amount);
                    return Convert.ToInt32(sumCalories / sumAmount);
                }
                else return 0;
            }
        }

        public Recipe() : base()
        {
            Ingredients = new List<RecipeEntry>();
        }
    }
}
