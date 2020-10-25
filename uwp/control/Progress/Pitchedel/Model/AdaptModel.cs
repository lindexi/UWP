using System;

namespace lindexi.uwp.Progress.Model
{
    public class AdaptModel
    {
        public AdaptModel(string name, Type viewModel)
        {
            Name = name;
            ViewModel = viewModel;
        }

        public string Name { get; set; }
        public Type ViewModel { get; set; }
    }
}
