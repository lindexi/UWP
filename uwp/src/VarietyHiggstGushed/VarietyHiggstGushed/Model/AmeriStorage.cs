using System.Collections.ObjectModel;

namespace VarietyHiggstGushed.Model
{
    public class AmeriStorage
    {
        public string Name { set; get; }

        public ObservableCollection<Property> PropertyStorage { set; get; } = new ObservableCollection<Property>();
    }
}