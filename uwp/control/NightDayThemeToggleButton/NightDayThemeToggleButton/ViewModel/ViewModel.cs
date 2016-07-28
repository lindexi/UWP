using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NightDayThemeToggleButton.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
        }

        public ElementTheme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                OnPropertyChanged();
            }
        }

        private ElementTheme _theme = ElementTheme.Light;
    }
}