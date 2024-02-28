using RecipeManager.Models;
using RecipeManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RecipeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
        }

        #region Ingredients
        private void AddIngredientCategoryBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IngredientCategoryNameTXT.Text))
            {
                MessageBox.Show("Bitte gebe eine Zutaten-Kategorie ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MainPageViewModel.Instance.Manager.IngredientCategories.Find(ic => ic == IngredientCategoryNameTXT.Text) != null)
            {
                MessageBox.Show($"Die Kategorie {IngredientCategoryNameTXT.Text} existiert bereits.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var newIngredientCategory = IngredientCategoryNameTXT.Text;
            MainPageViewModel.Instance.Manager.IngredientCategories.Add(newIngredientCategory);
            Manager.WriteData(MainPageViewModel.Instance.Manager);
            IngredientCategoryNameTXT.Text = "";
            MainPageViewModel.Instance.Manager = Manager.ReadData();
        }

        private void DeleteIngredientCategoryBTN_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientCategoryLB.SelectedItem != null)
            {
                var ingredientCategories = IngredientCategoryLB.SelectedItems;
                var ingredientCategoryNames = new List<string>();
                foreach (string ingredientCategory in ingredientCategories)
                {
                    ingredientCategoryNames.Add(ingredientCategory);
                }

                var result = MessageBoxResult.No;
                if (ingredientCategories.Count == 1)
                    result = MessageBox.Show($"Möchten Sie die Kategorie {ingredientCategoryNames[0]} wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                else result = MessageBox.Show($"Möchten Sie die folgenden Kategorien wirklich löschen?\n{string.Join("\n", ingredientCategoryNames)}", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (string ingredientCategory in ingredientCategoryNames)
                    {
                        MainPageViewModel.Instance.Manager.IngredientCategories.Remove(ingredientCategory);
                        Manager.WriteData(MainPageViewModel.Instance.Manager);
                        MainPageViewModel.Instance.Manager = Manager.ReadData();
                    }
                }
            }
        }

        private void IngredientCategoryLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IngredientCategoryLB.SelectedItem != null)
            {
                MainPageViewModel.Instance.DeleteIngredientCategoryVisiblity = Visibility.Visible;
            }
            else
            {
                MainPageViewModel.Instance.DeleteIngredientCategoryVisiblity = Visibility.Collapsed;
            }
        }

        private void CreateOrEditIngredientBTN_Click(object sender, RoutedEventArgs e)
        {
            var targetIngredient = IngredientDG.SelectedItem as Ingredient;
            if (targetIngredient == null)
            {
                if (string.IsNullOrEmpty(IngredientNameTXT.Text))
                {
                    MessageBox.Show("Bitte gebe einen Zutaten-Namen ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var calories = 0;
            if (!string.IsNullOrEmpty(IngredientCaloriesTXT.Text))
            {
                if (!Int32.TryParse(IngredientCaloriesTXT.Text, out calories))
                {
                    MessageBox.Show("Bitte gebe eine Zahl für die Kalorien ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (IngredientCategoryCMB.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle eine Zutaten-Kagegorie aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (targetIngredient == null)
            {
                var newIngredient = new Ingredient() { Name = IngredientNameTXT.Text, Calories = calories, Comment = IngredientCommentTXT.Text, Category = IngredientCategoryCMB.SelectedItem.ToString() };
                MainPageViewModel.Instance.Manager.Ingredients.Add(newIngredient);
            }
            else
            {
                var foundIngredient = MainPageViewModel.Instance.Manager.Ingredients.Find(i => i.Name == targetIngredient.Name);
                foundIngredient.Calories = calories;
                foundIngredient.Comment = IngredientCommentTXT.Text;
                foundIngredient.Category = IngredientCategoryCMB.SelectedItem.ToString();
            }

            Manager.WriteData(MainPageViewModel.Instance.Manager);
            MainPageViewModel.Instance.Manager = Manager.ReadData();
        }

        private void DeleteIngredientBTN_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientDG.SelectedItem != null)
            {
                var ingredients = IngredientDG.SelectedItems;
                var ingredientNames = new List<string>();
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientNames.Add(ingredient.Name);
                }

                var result = MessageBoxResult.No;
                if (ingredients.Count == 1)
                    result = MessageBox.Show($"Möchten Sie die Zutat {((Ingredient)ingredients[0]).Name} wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                else result = MessageBox.Show($"Möchten Sie die folgenden Zutaten wirklich löschen?\n{string.Join("\n", ingredientNames)}", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (string ingredient in ingredientNames)
                    {
                        var targetIngredient = MainPageViewModel.Instance.Manager.Ingredients.Find(i => i.Name == ingredient);
                        if (targetIngredient != null)
                        {
                            MainPageViewModel.Instance.Manager.Ingredients.Remove(targetIngredient);
                            Manager.WriteData(MainPageViewModel.Instance.Manager);
                            MainPageViewModel.Instance.Manager = Manager.ReadData();
                        }
                    }
                }
            }
        }

        private void IngredientDG_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IngredientDG.SelectedItem != null)
            {
                MainPageViewModel.Instance.IngredientMode = Mode.Delete;
                if (IngredientDG.SelectedItems.Count == 1)
                {
                    var ingredient = (Ingredient)IngredientDG.SelectedItem;
                    IngredientNameTXT.Text = ingredient.Name;
                    IngredientNameTXT.IsEnabled = false;
                    IngredientCategoryCMB.SelectedItem = ingredient.Category;
                    IngredientCaloriesTXT.Text = ingredient.Calories.ToString();
                    IngredientCommentTXT.Text = ingredient.Comment;

                    MainPageViewModel.Instance.IngredientMode = Mode.Edit;
                }
            }
            else
            {
                MainPageViewModel.Instance.IngredientMode = Mode.Create;
                IngredientNameTXT.Text = "";
                IngredientNameTXT.IsEnabled = true;
                IngredientCategoryCMB.SelectedItem = null;
                IngredientCaloriesTXT.Text = "";
                IngredientCommentTXT.Text = "";
            }
        }
        #endregion

        #region Recipes
        private void AddRecipeCategoryBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RecipeCategoryNameTXT.Text))
            {
                MessageBox.Show("Bitte gebe eine Rezept-Kategorie ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MainPageViewModel.Instance.Manager.RecipeCategories.Find(ic => ic == RecipeCategoryNameTXT.Text) != null)
            {
                MessageBox.Show($"Die Kategorie {RecipeCategoryNameTXT.Text} existiert bereits.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var newRecipeCategory = RecipeCategoryNameTXT.Text;
            MainPageViewModel.Instance.Manager.RecipeCategories.Add(newRecipeCategory);
            Manager.WriteData(MainPageViewModel.Instance.Manager);
            RecipeCategoryNameTXT.Text = "";
            MainPageViewModel.Instance.Manager = Manager.ReadData();
        }

        private void DeleteRecipeCategoryBTN_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeCategoryDG.SelectedItem != null)
            {
                var recipeCategories = RecipeCategoryDG.SelectedItems;
                var recipeCategoryNames = new List<string>();
                foreach (string recipeCategory in recipeCategories)
                {
                    recipeCategoryNames.Add(recipeCategory);
                }

                var result = MessageBoxResult.No;
                if (recipeCategories.Count == 1)
                    result = MessageBox.Show($"Möchten Sie die Kategorie {recipeCategoryNames[0]} wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                else result = MessageBox.Show($"Möchten Sie die folgenden Kategorien wirklich löschen?\n{string.Join("\n", recipeCategoryNames)}", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (string recipeCategory in recipeCategoryNames)
                    {
                        MainPageViewModel.Instance.Manager.RecipeCategories.Remove(recipeCategory);
                        Manager.WriteData(MainPageViewModel.Instance.Manager);
                        MainPageViewModel.Instance.Manager = Manager.ReadData();
                    }
                }
            }
        }

        private void RecipeCategoryDG_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RecipeCategoryDG.SelectedItem != null)
            {
                MainPageViewModel.Instance.DeleteRecipeCategoryVisiblity = Visibility.Visible;
            }
            else MainPageViewModel.Instance.DeleteRecipeCategoryVisiblity = Visibility.Collapsed;
        }

        private void CreateOrEditRecipeBTN_Click(object sender, RoutedEventArgs e)
        {
            var targetRecipe = RecipeDG.SelectedItem as Recipe;
            if (targetRecipe == null)
            {
                if (string.IsNullOrEmpty(RecipeNameTXT.Text))
                {
                    MessageBox.Show("Bitte gebe einen Rezept-Namen ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (RecipeCategoryCMB.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle eine Rezept-Kagegorie aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (targetRecipe == null)
            {
                var newRecipe = new Recipe() { Name = RecipeNameTXT.Text, Comment = RecipeCommentTXT.Text, Category = RecipeCategoryCMB.SelectedItem.ToString() };
                MainPageViewModel.Instance.Manager.Recipes.Add(newRecipe);
            }
            else
            {
                var foundRecipe = MainPageViewModel.Instance.Manager.Recipes.Find(i => i.Name == targetRecipe.Name);
                foundRecipe.Comment = RecipeCommentTXT.Text;
                foundRecipe.Category = RecipeCategoryCMB.SelectedItem.ToString();
            }

            MainPageViewModel.Instance.Manager.CurrentIngredients.Clear();
            Manager.WriteData(MainPageViewModel.Instance.Manager);
            MainPageViewModel.Instance.Manager = Manager.ReadData();
        }

        private void DeleteRecipeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeDG.SelectedItem != null)
            {
                var recipes = RecipeDG.SelectedItems;
                var recipeNames = new List<string>();
                foreach (Recipe recipe in recipes)
                {
                    recipeNames.Add(recipe.Name);
                }

                var result = MessageBoxResult.No;
                if (recipes.Count == 1)
                    result = MessageBox.Show($"Möchten Sie das Rezept {((Recipe)recipes[0]).Name} wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                else result = MessageBox.Show($"Möchten Sie die folgenden Rezepte wirklich löschen?\n{string.Join("\n", recipeNames)}", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (string recipe in recipeNames)
                    {
                        var targetRecipe = MainPageViewModel.Instance.Manager.Recipes.Find(i => i.Name == recipe);
                        if (targetRecipe != null)
                        {
                            MainPageViewModel.Instance.Manager.Recipes.Remove(targetRecipe);
                            Manager.WriteData(MainPageViewModel.Instance.Manager);
                            MainPageViewModel.Instance.Manager = Manager.ReadData();
                        }
                    }
                }
            }
        }

        private void RecipeDG_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RecipeDG.SelectedItem != null)
            {
                MainPageViewModel.Instance.RecipeMode = Mode.Delete;
                ImportEX.Visibility = Visibility.Collapsed;
                if (RecipeDG.SelectedItems.Count == 1)
                {
                    var recipe = (Recipe)RecipeDG.SelectedItem;
                    RecipeNameTXT.Text = recipe.Name;
                    RecipeNameTXT.IsEnabled = false;
                    RecipeCategoryCMB.SelectedItem = recipe.Category;
                    RecipeCommentTXT.Text = recipe.Comment;
                    MainPageViewModel.Instance.Manager.CurrentIngredients = recipe.Ingredients.OrderByDescending(i => i.Amount).ToList();

                    MainPageViewModel.Instance.RecipeMode = Mode.Edit;
                    ImportEX.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MainPageViewModel.Instance.RecipeMode = Mode.Create;
                RecipeNameTXT.Text = "";
                RecipeNameTXT.IsEnabled = true;
                RecipeCategoryCMB.SelectedItem = null;
                RecipeCommentTXT.Text = "";

                ImportEX.Visibility = Visibility.Visible;
            }
        }

        private void CreateOrEditCurrentIngredientBTN_Click(object sender, RoutedEventArgs e)
        {
            var targetCurrentIngredient = CurrentIngredientDG.SelectedItem as RecipeEntry;
            var amount = 0.0;
            if (targetCurrentIngredient == null)
            {
                if (!string.IsNullOrEmpty(CurrentIngredientAmountTXT.Text))
                {
                    if (!Double.TryParse(CurrentIngredientAmountTXT.Text, out amount))
                    {
                        MessageBox.Show("Bitte gebe eine Zahl für die Menge ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            if (CurrentIngredientCMB.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle eine Zutat aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (targetCurrentIngredient == null)
            {
                var newCurrentIngredient = new RecipeEntry((Ingredient)CurrentIngredientCMB.SelectedItem, amount);
                MainPageViewModel.Instance.Manager.CurrentIngredients.Add(newCurrentIngredient);
            }
            else
            {
                var foundCurrentIngredient = MainPageViewModel.Instance.Manager.CurrentIngredients.Find(i => i.Ingredient.Name == targetCurrentIngredient.Ingredient.Name);
                foundCurrentIngredient.Amount = amount;
                foundCurrentIngredient.Ingredient = (Ingredient)CurrentIngredientCMB.SelectedItem;
            }

            Manager.WriteData(MainPageViewModel.Instance.Manager);
            MainPageViewModel.Instance.Manager = Manager.ReadData();
        }

        private void DeleteCurrentIngredientBTN_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIngredientDG.SelectedItem != null)
            {
                var currentIngredients = CurrentIngredientDG.SelectedItems;
                var currentIngredientNames = new List<string>();
                foreach (RecipeEntry currentIngredient in currentIngredients)
                {
                    currentIngredientNames.Add(currentIngredient.Ingredient.Name);
                }

                var result = MessageBoxResult.No;
                if (currentIngredients.Count == 1)
                    result = MessageBox.Show($"Möchten Sie das Rezept {((RecipeEntry)currentIngredients[0]).Ingredient.Name} wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                else result = MessageBox.Show($"Möchten Sie die folgenden Rezepte wirklich löschen?\n{string.Join("\n", currentIngredientNames)}", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (string currentIngredient in currentIngredientNames)
                    {
                        var targetCurrentIngredient = MainPageViewModel.Instance.Manager.CurrentIngredients.Find(i => i.Ingredient.Name == currentIngredient);
                        if (targetCurrentIngredient != null)
                        {
                            MainPageViewModel.Instance.Manager.CurrentIngredients.Remove(targetCurrentIngredient);
                            Manager.WriteData(MainPageViewModel.Instance.Manager);
                            MainPageViewModel.Instance.Manager = Manager.ReadData();
                        }
                    }
                }
            }
        }

        private void CurrentIngredientDG_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CurrentIngredientDG.SelectedItem != null)
            {
                MainPageViewModel.Instance.CurrentIngredientMode = Mode.Delete;
                if (CurrentIngredientDG.SelectedItems.Count == 1)
                {
                    var currentIngredient = (RecipeEntry)CurrentIngredientDG.SelectedItem;
                    CurrentIngredientCMB.SelectedItem = currentIngredient.Ingredient;
                    CurrentIngredientCMB.IsReadOnly = true;
                    CurrentIngredientAmountTXT.Text = currentIngredient.Amount.ToString();

                    MainPageViewModel.Instance.CurrentIngredientMode = Mode.Edit;
                }
            }
            else
            {
                MainPageViewModel.Instance.CurrentIngredientMode = Mode.Create;
                CurrentIngredientCMB.SelectedItem = null;
                CurrentIngredientCMB.IsReadOnly = false;
                CurrentIngredientAmountTXT.Text = "";
            }
        }

        private void ImportIngredientsBTN_Click(object sender, RoutedEventArgs e)
        {
            if (GetIngredientsFromCMB.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle ein Rezept aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var targetRecipe = GetIngredientsFromCMB.SelectedItem as Recipe;
            if (targetRecipe != null && targetRecipe.Ingredients != null && targetRecipe.Ingredients.Any())
            {
                targetRecipe.Ingredients.ForEach(i => MainPageViewModel.Instance.Manager.CurrentIngredients.Add(i));
                Manager.WriteData(MainPageViewModel.Instance.Manager);
                MainPageViewModel.Instance.Manager = Manager.ReadData();
            }
            else MessageBox.Show("Keine Zutaten für den Import vorhanden.", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
