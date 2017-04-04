using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public class ViewModelAttribute : Attribute
    {
        public Type ViewModel { get; set; }
    }
}