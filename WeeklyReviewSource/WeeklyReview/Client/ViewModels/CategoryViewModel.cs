using System.Drawing;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Client.ViewModels
{
    public class CategoryViewModel
    {
        private CategoryModel _model;

        public CategoryViewModel()
        {
            _model = new CategoryModel();
        }

        public CategoryViewModel(CategoryModel model)
        {
            _model = model;
        }

        public CategoryModel CloneModel() => (CategoryModel) _model.Clone();
        public CategoryModel GetModel() => _model;

        public string Name => _model.Name;
        public Color Color => _model.Color;
        public int Priority
        {
            get =>_model.Priority;
            set
            {
                _model.Priority = value;
            }
        }

        public string ColorHex
        {
            get => HexConverter(_model.Color);
            set
            {
                var colorInt = int.Parse(value.Substring(1), System.Globalization.NumberStyles.HexNumber);
                _model.Color = Color.FromArgb(colorInt);
            }
        }

        private static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
