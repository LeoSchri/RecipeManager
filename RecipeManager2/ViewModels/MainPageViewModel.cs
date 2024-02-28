using RecipeManager.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RecipeManager.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public static MainPageViewModel Instance { get; set; }

        private Manager _manager;
        public Manager Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                OnPropertyChanged();
            }
        }

        private Mode _ingredientMode = Mode.Create;
        public Mode IngredientMode
        {
            get { return _ingredientMode; }
            set
            {
                _ingredientMode = value;
                if (_ingredientMode == Mode.Delete)
                    DeleteIngredientVisibility = Visibility.Visible;
                else if (_ingredientMode == Mode.Edit)
                {
                    DeleteIngredientVisibility = Visibility.Visible;
                    IngredientModeText = "Speichern";
                }
                else
                {
                    IngredientModeText = "Hinzufügen";
                    DeleteIngredientVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        private string _ingredientModeText = "Hinzufügen";
        public string IngredientModeText
        {
            get { return _ingredientModeText; }
            set
            {
                _ingredientModeText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _deleteIngredientVisibility = Visibility.Collapsed;
        public Visibility DeleteIngredientVisibility
        {
            get { return _deleteIngredientVisibility; }
            set
            {
                _deleteIngredientVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _deleteIngredientCategoryVisiblity = Visibility.Collapsed;
        public Visibility DeleteIngredientCategoryVisiblity
        {
            get { return _deleteIngredientCategoryVisiblity; }
            set
            {
                _deleteIngredientCategoryVisiblity = value;
                OnPropertyChanged();
            }
        }

        private Mode _recipeMode = Mode.Create;
        public Mode RecipeMode
        {
            get { return _recipeMode; }
            set
            {
                _recipeMode = value;
                if (_recipeMode == Mode.Delete)
                    DeleteRecipeVisibility = Visibility.Visible;
                else if (_recipeMode == Mode.Edit)
                {
                    DeleteRecipeVisibility = Visibility.Visible;
                    RecipeModeText = "Speichern";
                }
                else
                {
                    RecipeModeText = "Hinzufügen";
                    DeleteRecipeVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        private string _recipeModeText = "Hinzufügen";
        public string RecipeModeText
        {
            get { return _recipeModeText; }
            set
            {
                _recipeModeText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _deleteRecipeCategoryVisiblity = Visibility.Collapsed;
        public Visibility DeleteRecipeCategoryVisiblity
        {
            get { return _deleteRecipeCategoryVisiblity; }
            set
            {
                _deleteRecipeCategoryVisiblity = value;
                OnPropertyChanged();
            }
        }

        private Visibility _deleteRecipeVisiblity = Visibility.Collapsed;
        public Visibility DeleteRecipeVisibility
        {
            get { return _deleteRecipeVisiblity; }
            set
            {
                _deleteRecipeVisiblity = value;
                OnPropertyChanged();
            }
        }

        private Mode _currentIngredientMode = Mode.Create;
        public Mode CurrentIngredientMode
        {
            get { return _currentIngredientMode; }
            set
            {
                _currentIngredientMode = value;
                if (_currentIngredientMode == Mode.Delete)
                    DeleteCurrentIngredientVisibility = Visibility.Visible;
                else if (_currentIngredientMode == Mode.Edit)
                {
                    DeleteCurrentIngredientVisibility = Visibility.Visible;
                    CurrentIngredientModeText = "Speichern";
                }
                else
                {
                    CurrentIngredientModeText = "Hinzufügen";
                    DeleteCurrentIngredientVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        private string _currentIngredientModeText = "Hinzufügen";
        public string CurrentIngredientModeText
        {
            get { return _currentIngredientModeText; }
            set
            {
                _currentIngredientModeText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _deleteCurrentIngredientVisiblity = Visibility.Collapsed;
        public Visibility DeleteCurrentIngredientVisibility
        {
            get { return _deleteCurrentIngredientVisiblity; }
            set
            {
                _deleteCurrentIngredientVisiblity = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            Manager = Manager.ReadData();
            Instance = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
