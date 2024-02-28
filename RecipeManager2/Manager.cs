using Newtonsoft.Json;
using RecipeManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RecipeManager
{
    internal class Manager : INotifyPropertyChanged
    {
        [JsonIgnore]
        public static string JsonPath { get; set; }

        private List<Ingredient> _ingredients;
        public List<Ingredient> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value;
                OnPropertyChanged();
            }
        }

        private List<Recipe> _recipes;
        public List<Recipe> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged();
            }
        }

        private List<RecipeEntry> _currentIngredients;
        public List<RecipeEntry> CurrentIngredients
        {
            get { return _currentIngredients; }
            set
            {
                _currentIngredients = value;
                OnPropertyChanged();
            }
        }

        private List<string> _ingredientCategories;
        public List<string> IngredientCategories
        {
            get { return _ingredientCategories; }
            set
            {
                _ingredientCategories = value;
                OnPropertyChanged();
            }
        }

        private List<string> _recipeCategories;
        public List<string> RecipeCategories
        {
            get { return _recipeCategories; }
            set
            {
                _recipeCategories = value;
                OnPropertyChanged();
            }
        }

        public Manager()
        {
            Ingredients = new List<Ingredient>();
            Recipes = new List<Recipe>();
            IngredientCategories = new List<string>();
            RecipeCategories = new List<string>();
            CurrentIngredients = new List<RecipeEntry>();

            var projectFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RecipeManager";
            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);
            JsonPath = projectFolder + @"\RecipeManager.json";
        }

        public static Manager ReadData()
        {
            var newManager = new Manager();
            if (File.Exists(JsonPath))
            {
                var json = File.ReadAllText(JsonPath);
                newManager = JsonConvert.DeserializeObject<Manager>(json);
            }
            return newManager;
        }

        public static void WriteData(Manager manager)
        {
            manager.Ingredients = manager.Ingredients.OrderBy(i => i.Name).ToList();
            manager.Recipes = manager.Recipes.OrderBy(r => r.Name).ToList();
            manager.IngredientCategories = manager.IngredientCategories.OrderBy(ic => ic).ToList();
            manager.RecipeCategories = manager.RecipeCategories.OrderBy(rc => rc).ToList();
            manager.CurrentIngredients = manager.CurrentIngredients.OrderByDescending(ci => ci.Amount).ToList();

            var json = JsonConvert.SerializeObject(manager, Formatting.Indented);
            File.WriteAllText(JsonPath, json);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
